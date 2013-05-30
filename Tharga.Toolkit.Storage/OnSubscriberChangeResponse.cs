using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public class OnSubscriberChangeResponse
    {
        [DataMember]
        public int SubscriberCount { get; set; }

        [DataMember]
        public string ClientAddress { get; set; }

        [DataMember]
        public string ServerAddress { get; set; }
    }
}
