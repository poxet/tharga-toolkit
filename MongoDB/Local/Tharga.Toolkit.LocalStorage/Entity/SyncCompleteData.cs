using System;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public class SyncCompleteData : EventArgs, ISyncCompleteData
    {
        public DateTime? LastServerStoreTime { get; private set; }
        public int EntityChangedCount { get; private set; }
        public int EntityDeletedCount { get; private set; }

        public SyncCompleteData(int entityChangedCount, int entityDeletedCount, DateTime? lastServerStoreTime)
        {
            EntityChangedCount = entityChangedCount;
            EntityDeletedCount = entityDeletedCount;
            LastServerStoreTime = lastServerStoreTime;
        }
    }
}