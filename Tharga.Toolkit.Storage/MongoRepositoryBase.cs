using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using MongoDB.Driver;

namespace Tharga.Toolkit.Storage
{
    public abstract class MongoRepositoryBase
    {
        private static readonly object SyncRoot = new object();
        private static MongoDatabase _database;
        private static readonly Dictionary<string, MongoCollection> CollectionCache = new Dictionary<string, MongoCollection>();

        private static MongoDatabase GetDatabase(string realmName)
        {
            if (_database != null)
                return _database;

            lock (SyncRoot)
            {
                if (_database == null)
                {
                    var client = new MongoClient();
                    var server = client.GetServer();

                    //NOTE: Implement special configuration settings
                    var databaseName = ConfigurationManager.AppSettings["MongoDBName"];
                    if (string.IsNullOrEmpty(databaseName))
                        throw new ConfigurationErrorsException("There is no name specified for the MongoDB in the config file. Enter a setting with key 'MongoDBName' under the appSettings section.");

                    if (realmName != null)
                        databaseName += "_" + realmName;

                    _database = server.GetDatabase(databaseName);

                    try
                    {
                        var stats = _database.GetStats();
                        Debug.WriteLine("There are {0} collections in the database.", stats.CollectionCount);
                    }
                    catch (MongoConnectionException exception)
                    {
                        _database = null;
                        throw new DatabaseOfflineException("The mongo database is offline.", exception, databaseName);
                    }
                    catch (Exception)
                    {
                        _database = null;
                        throw;
                    }
                }
            }


            return _database;
        }

        public MongoCollection GetDeleteCollection()
        {
            const string collectionName = "Deleted";

            if (!CollectionCache.ContainsKey(collectionName))
            {
                lock (SyncRoot)
                {
                    if (!CollectionCache.ContainsKey(collectionName))
                    {
                        var collection = GetDatabase(null).GetCollection(collectionName);
                        CollectionCache.Add(collectionName, collection);
                    }
                }
            }

            var deleteCollection = CollectionCache[collectionName];
            if (deleteCollection == null) throw new NullReferenceException(string.Format("The delete collection {0} does not exist.", collectionName));
            return deleteCollection;
        }

        public MongoCollection<T> GetCollection<T>()
        {
            var collectionName = GetCollectionName<T>();

            if (!CollectionCache.ContainsKey(collectionName))
            {
                lock (SyncRoot)
                {
                    if (!CollectionCache.ContainsKey(collectionName))
                    {
                        var collection = GetDatabase(null).GetCollection<T>(collectionName);
                        CollectionCache.Add(collectionName, collection);
                    }
                }
            }

            return CollectionCache[collectionName] as MongoCollection<T>;
        }

        private static string GetCollectionName<T>()
        {
            string collectionName = null;
            foreach (var attribute in (typeof(T).GetCustomAttributes(true)))
            {
                var mongoDbCollectionAttribute = attribute as MongoDBCollectionAttribute;
                if (mongoDbCollectionAttribute != null)
                    collectionName = (mongoDbCollectionAttribute).Name;
            }
            return collectionName ?? typeof(T).ToShortString();
        }
    }
}