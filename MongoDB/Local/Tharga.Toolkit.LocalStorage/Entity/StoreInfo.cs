using System;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public class StoreInfo
    {
        public DateTime? ServerStoreTime { get; private set; }
        public DateTime? LocalStoreTime { get; private set; }
        public string LocalMachineName { get; private set; }
        public string LocalUserName { get; private set; }

        public void SetLocalSaveInfo()
        {
            LocalStoreTime = DateTime.UtcNow;
            LocalMachineName = System.Environment.MachineName;
            LocalUserName = string.Format(@"{0}\{1}", System.Environment.UserDomainName, System.Environment.UserName);
        }

        public bool IsInSync
        {
            get
            {
                if (ServerStoreTime == null) 
                    return false;

                if (LocalStoreTime > ServerStoreTime)
                    return false;

                return true;
            }
        }

        public static StoreInfo Build(DateTime serverStoreTime, DateTime localStoreTime, string localMachineName, string localUserName)
        {
            return new StoreInfo
                       {
                           ServerStoreTime = serverStoreTime,
                           LocalMachineName = localMachineName,
                           LocalStoreTime = localStoreTime,
                           LocalUserName = localUserName,
                       };
        }
    }
}