using System;
using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public class SyncRequest
    {
        [DataMember]
        public DateTime? ServerStoreTime { get; set; }
    }
}