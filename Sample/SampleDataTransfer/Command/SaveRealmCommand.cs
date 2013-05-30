using System.Runtime.Serialization;
using SampleDataTransfer.Entities;

namespace SampleDataTransfer.Command
{
    [DataContract]
    public class SaveRealmCommand
    {
        [DataMember]
        public RealmDto Realm { get; set; }

        [DataMember]
        public bool NotifySubscribers { get; set; }        
    }
}