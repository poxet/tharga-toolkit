using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Repository
{
    public abstract class LocalRepositoryBase<TEntity> : ILocalRepository<TEntity>
        where TEntity : IEntity
    {
        private readonly MongoRepository _mongoRepository;

        protected LocalRepositoryBase()
        {
            _mongoRepository = new MongoRepository();
        }

        protected MongoCollection<TEntity> GetCollection(Guid realmId)
        {
            throw new NotImplementedException();
            //return _mongoRepository.GetCollection<TEntity>();
        }

        public void Delete(Guid realmId, Guid id)
        {
            _mongoRepository.Delete<TEntity>(realmId, id);
        }

        public void DeleteAll(Guid realmId)
        {
            _mongoRepository.DeleteAll<TEntity>(realmId);
        }

        public IEnumerable<TEntity> GetAll(Guid realmId)
        {
            return _mongoRepository.GetAll<TEntity>(realmId);
        }

        public TEntity Get(Guid realmId, Guid id)
        {
            return _mongoRepository.Get<TEntity>(realmId, id);
        }

        public IEnumerable<TEntity> GetOutOfSync(Guid realmId)
        {
            return _mongoRepository.GetAll<TEntity>(realmId).Where(x => !x.StoreInfo.IsInSync);
        }

        public IEnumerable<TEntity> GetInSync(Guid realmId)
        {
            return _mongoRepository.GetAll<TEntity>(realmId).Where(x => x.StoreInfo.IsInSync);
        }

        public void Save(Guid realmId, TEntity entity)
        {
            _mongoRepository.Save(realmId, entity);
        }
    }
}