using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public class SyncCommand : ICommand
    {
        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public SyncRequest SyncRequest { get; set; }
    }
}