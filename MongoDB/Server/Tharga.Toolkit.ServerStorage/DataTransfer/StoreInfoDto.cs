using System;

namespace Tharga.Toolkit.ServerStorage.DataTransfer
{
    public class StoreInfoDto
    {
        public DateTime? ServerStoreTime { get; set; }
        public DateTime? LocalStoreTime { get; set; }
        public string LocalMachineName { get; set; }
        public string LocalUserName { get; set; }

        public static StoreInfoDto GetServerStoreInfo()
        {
            return new StoreInfoDto
            {
                LocalStoreTime = DateTime.UtcNow,
                LocalMachineName = string.Empty,
                LocalUserName = "Service",
                ServerStoreTime = null
            };
        }
    }
}