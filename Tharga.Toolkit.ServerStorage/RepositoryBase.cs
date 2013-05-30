using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.ServerStorage.Repository;

namespace Tharga.Toolkit.ServerStorage
{
    public abstract class RepositoryBase<TEntity>
        where TEntity : IOutputDto
    {
        private readonly MongoRepository _database;

        public void EnsureUniqueIndex(IEnumerable<string> propertyNames)
        {
            var names = propertyNames.Select(x => string.Format("Item.{0}", x)).ToArray();
            if (!_database.GetCollection<TEntity>().IndexExists(names))
                _database.GetCollection<TEntity>().EnsureIndex(new IndexKeysBuilder().Ascending(names), IndexOptions.SetUnique(true));
        }

        public void Save(Guid realmId, TEntity entity)
        {
            try
            {
                var collection = _database.GetCollection<TEntity>();
                collection.Save(DataItem<TEntity>.Create(realmId, entity), WriteConcern.Acknowledged);
            }
            catch (WriteConcernException exception)
            {
                //TODO: Perhaps replace with a specific exception
                throw;
            }
        }

        public TEntity Delete(Guid realmId, Guid id, DateTime serverDeleteTime)
        {
            var entity = Get(realmId, id);
            entity.StoreInfo.ServerStoreTime = serverDeleteTime;
            var deleteEntity = DataItem<TEntity>.Create(realmId, entity);
            var deleteCollection = _database.GetDeleteCollection();
            deleteCollection.Save(deleteEntity, WriteConcern.Acknowledged);

            var query = Query.And(Query.EQ("_id", id), Query.EQ("RealmId", realmId));
            _database.GetCollection<TEntity>().Remove(query);

            return entity;
        }

        public TEntity Get(Guid realmId, Guid id)
        {
            var query = Query.And(Query.EQ("_id", id), Query.EQ("RealmId", realmId));
            var list = _database.GetCollection<TEntity>().FindAs<DataItem<TEntity>>(query).Select(x => x.Item);
            return list.FirstOrDefault();
        }

        public virtual long Count(Guid realmId)
        {
            var query = Query.And(Query.EQ("RealmId", realmId));
            return _database.GetCollection<TEntity>().Count(query);
        }

        public virtual IEnumerable<TEntity> GetAll(Guid realmId)
        {
            var query = Query.And(Query.EQ("RealmId", realmId));
            var list = _database.GetCollection<TEntity>().FindAs<DataItem<TEntity>>(query).Select(x => x.Item);
            return list;
        }

        public IEnumerable<TEntity> GetAllFrom(Guid realmId, DateTime? syncTime)
        {
            var query = Query.And(Query.EQ("RealmId", realmId));
            if (syncTime != null)
                query = Query.And(Query.EQ("RealmId", realmId), Query.GT("Item.StoreInfo.ServerStoreTime", syncTime));

            var list = _database.GetCollection<TEntity>().FindAs<DataItem<TEntity>>(query).Select(x => x.Item);
            return list;
        }

        public IEnumerable<TEntity> GetDeletedFrom(Guid realmId, DateTime? syncTime)
        {
            var query = Query.And(Query.EQ("RealmId", realmId));
            if (syncTime != null)
                query = Query.And(Query.EQ("RealmId", realmId), Query.GT("Item.StoreInfo.ServerStoreTime", syncTime));

            var collection = _database.GetDeleteCollection();
            if (collection == null)
                return new List<TEntity>();

            return collection.FindAs<DataItem<TEntity>>(query).Select(x => x.Item);
        }

        protected RepositoryBase(MongoRepository database)
        {
            _database = database;
        }

        public DateTime? GetLastServerStoreTime(Guid realmId)
        {
            var list = _database.GetCollection<TEntity>().FindAllAs<DataItem<TEntity>>().Where(x => x.RealmId == realmId).Select(x => x.Item).ToList();
            return !list.Any() ? null : list.Max(x => x.StoreInfo.ServerStoreTime);
        }
    }
}