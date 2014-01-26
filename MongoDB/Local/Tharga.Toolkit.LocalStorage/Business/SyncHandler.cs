using System;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.LocalStorage.Repository;

namespace Tharga.Toolkit.LocalStorage.Business
{
    class SyncHandler
    {
        private ISyncLocalRepository _localRepositoryInstance;

        internal ISyncLocalRepository LocalRepositoryInstance
        {
            get { return _localRepositoryInstance ?? (_localRepositoryInstance = new SyncLocalRepository()); }
            set { _localRepositoryInstance = value; }
        }

        public DateTime? GetSyncTime(Guid realmId, Type type)
        {
            return LocalRepositoryInstance.GetSyncTime(realmId, type);
        }

        public void SetSyncTime(Guid realmId, Type type, DateTime syncTime)
        {
            LocalRepositoryInstance.SetSyncTime(realmId, type, syncTime);
        }

        public void ClearSyncTime(Guid realmId, Type type)
        {
            LocalRepositoryInstance.ClearSyncTime(realmId, type);
        }
    }
}