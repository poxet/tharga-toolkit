using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.LocalStorage.Repository
{
    public class MongoRepository : MongoRepositoryBase
    {
        internal MongoRepository()
        {

        }

        public void DeleteAll<T>(Guid realmId)
        {
            if (!GetCollection<T>().Exists())
                return;

            var query = Query.EQ("RealmId", realmId);
            GetCollection<T>().Remove(query);
        }

        public IEnumerable<T> GetAll<T>(Guid realmId) 
            where T : IId
        {
            var query = Query.EQ("RealmId", realmId);
            var items = GetCollection<T>().FindAs<RealmData<T>>(query);
            return items.Select(x => x.Entity);
        }

        public T Get<T>(Guid realmId, Guid id) 
            where T : IId
        {
            var query = Query.And(Query.EQ("_id", id), Query.EQ("RealmId", realmId));
            var item = GetCollection<T>().FindAs<RealmData<T>>(query).FirstOrDefault();
            return item == null ? default(T) : item.Entity;
        }

        public void Save<T>(Guid realmId, T entity)
            where T : IId
        {
            GetCollection<T>().Save(new RealmData<T>(realmId, entity));
        }

        public void Delete<T>(Guid realmId, Guid id)
        {
            var query = Query.And(Query.EQ("_id", id), Query.EQ("RealmId", realmId));
            GetCollection<T>().Remove(query);
        }
    }
}