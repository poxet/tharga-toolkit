using System;

namespace Tharga.Toolkit.LocalStorage.Interface
{
    interface ISyncLocalRepository
    {
        DateTime? GetSyncTime(Guid realmId, Type type);
        void SetSyncTime(Guid realmId, Type type, DateTime syncTime);
        void ClearSyncTime(Guid realmId, Type type);
    }
}