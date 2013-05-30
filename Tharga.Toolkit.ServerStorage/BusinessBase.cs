using System;
using System.Collections.Generic;
using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.ServerStorage.Repository;

namespace Tharga.Toolkit.ServerStorage
{
    public abstract class BusinessBase<TOutput, TInput>
        where TOutput : IOutputDto
        where TInput : IInputDto
    {
        protected GenericRepository<TOutput> RepositoryInstance { get; private set; }

        protected BusinessBase()
        {
            RepositoryInstance = new GenericRepository<TOutput>();
        }

        protected abstract TOutput Convert(TInput input);

        public DateTime? GetLastServerStoreTime(Guid realmId)
        {
            return RepositoryInstance.GetLastServerStoreTime(realmId);
        }

        public virtual TOutput Save(Guid realmId, TInput input)
        {
            if (input.StoreInfo == null) throw new ArgumentNullException("input", "StoreInfo has not been assigned.");

            var output = Convert(input);
            output.StoreInfo.ServerStoreTime = DateTime.UtcNow;

            RepositoryInstance.Save(realmId, output);

            return output;
        }

        public virtual IEnumerable<TOutput> GetAll(Guid realmId)
        {
            return RepositoryInstance.GetAll(realmId);
        }

        public virtual IEnumerable<TOutput> GetSyncList(Guid realmId, DateTime? serverStoreTime)
        {
            return RepositoryInstance.GetAllFrom(realmId, serverStoreTime);
        }

        public virtual IEnumerable<TOutput> GetDeleted(Guid realmId, DateTime? serverStoreTime)
        {
            return RepositoryInstance.GetDeletedFrom(realmId, serverStoreTime);
        }

        public virtual TOutput Delete(Guid realmId, Guid id)
        {
            return RepositoryInstance.Delete(realmId, id, DateTime.UtcNow);
        }

        public virtual TOutput Get(Guid realmId, Guid id)
        {
            return RepositoryInstance.Get(realmId, id);
        }
    }
}
