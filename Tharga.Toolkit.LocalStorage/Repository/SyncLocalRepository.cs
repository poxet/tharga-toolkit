using System;
using System.Linq;
using Tharga.Toolkit.LocalStorage.Entity;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Repository
{
    public class SyncLocalRepository : ISyncLocalRepository
    {
        private readonly MongoRepository _mongoRepository;

        public SyncLocalRepository()
        {
            _mongoRepository = new MongoRepository();
        }

        public DateTime? GetSyncTime(Guid realmId, Type type)
        {
            var typeName = type.ToString();

            var syncData = _mongoRepository.GetAll<SyncData>(realmId).FirstOrDefault(x => x.Type == typeName);

            if (syncData == null) return null;
            return syncData.SyncTime;
        }

        public void SetSyncTime(Guid realmId, Type type, DateTime syncTime)
        {
            if (syncTime.Year == 1)
                throw new ArgumentException(string.Format("Invalid sync time was provided. ({0} {1})", syncTime.ToShortDateString(), syncTime.ToLongTimeString()));

            var typeName = type.ToString();

            var syncData = _mongoRepository.GetAll<SyncData>(realmId).FirstOrDefault(x => x.Type == typeName);
            if (syncData == null)
            {
                syncData = new SyncData
                               {
                                   Id = Guid.NewGuid(),
                                   Type = typeName,
                                   SyncTime = syncTime,
                               };
            }
            else
                syncData.SyncTime = syncTime;

            _mongoRepository.Save(realmId, syncData);
        }

        public void ClearSyncTime(Guid realmId, Type type)
        {
            var typeName = type.ToString();

            var syncData = _mongoRepository.GetAll<SyncData>(realmId).FirstOrDefault(x => x.Type == typeName);
            if (syncData != null)
                _mongoRepository.Delete<SyncData>(realmId, syncData.Id);
        }
    }
}