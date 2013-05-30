using System.Runtime.Serialization;
using Tharga.Toolkit.Storage;

namespace SampleDataTransfer.Command
{
    [DataContract]
    public class SyncRealmCommand
    {
        [DataMember]
        public SyncRequest SyncRequest { get; set; }
    }
}