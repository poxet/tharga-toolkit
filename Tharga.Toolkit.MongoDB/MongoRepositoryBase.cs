//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Diagnostics;
//using MongoDB.Driver;
//using Tharga.Toolkit.MongoDB.Exceptions;

//namespace Tharga.Toolkit.MongoDB
//{
//    public abstract class MongoRepositoryBase
//    {
//        private static readonly object SyncRoot = new object();
//        private static MongoDatabase _database;
//        private static readonly Dictionary<string, MongoCollection> CollectionCache = new Dictionary<string, MongoCollection>();

//        protected internal static MongoDatabase Database
//        {
//            get
//            {
//                if (_database != null)
//                    return _database;

//                lock (SyncRoot)
//                {
//                    if (_database == null)
//                    {
//                        var client = new MongoClient();
//                        var server = client.GetServer();
//                        //var server = MongoClient.GetServer();
//                        //var server = MongoServer.Create();
//                        //var server = MongoDB.Driver.MongoClient.GetServer();
//                        //var servers = MongoServer.GetAllServers();
//                        //var server = servers[0]; //TODO: Pick the correct server.

//                        //NOTE: Implement special configuration settings
//                        var databaseName = ConfigurationManager.AppSettings["MongoDBName"];
//                        if (string.IsNullOrEmpty(databaseName))
//                            throw new ConfigurationErrorsException("There is no name specified for the MongoDB in the config file. Enter a setting with key 'MongoDBName' under the appSettings section.");

//                        _database = server.GetDatabase(databaseName);

//                        try
//                        {
//                            var stats = _database.GetStats();
//                            Debug.WriteLine("There are {0} collections in the database.", stats.CollectionCount);
//                        }
//                        catch (MongoConnectionException exception)
//                        {
//                            _database = null;
//                            throw new DatabaseOfflineException("The mongo database is offline.", exception, databaseName);
//                        }
//                        catch (Exception)
//                        {
//                            _database = null;
//                            throw;
//                        }
//                    }
//                }


//                return _database;
//            }
//        }

//        public MongoCollection<T> GetCollection<T>()
//        {
//            var collectionName = GetCollectionName<T>();

//            if (!CollectionCache.ContainsKey(collectionName))
//            {
//                lock (SyncRoot)
//                {
//                    if (!CollectionCache.ContainsKey(collectionName))
//                    {
//                        var collection = Database.GetCollection<T>(collectionName);
//                        CollectionCache.Add(collectionName, collection);
//                    }
//                }
//            }

//            return CollectionCache[collectionName] as MongoCollection<T>;
//        }

//        private static string GetCollectionName<T>()
//        {
//            string collectionName = null;
//            foreach (var attribute in (typeof(T).GetCustomAttributes(true)))
//            {
//                var mongoDbCollectionAttribute = attribute as MongoDBCollectionAttribute;
//                if (mongoDbCollectionAttribute != null)
//                    collectionName = (mongoDbCollectionAttribute).Name;
//            }
//            return collectionName ?? typeof(T).ToShortString();
//        }
//    }
//}


