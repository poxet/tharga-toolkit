using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public class OnSubscriptionCheckedResponse
    {
        [DataMember]
        public bool Restarted { get; set; }

        [DataMember]
        public string ClientAddress { get; set; }

        [DataMember]
        public string ServerAddress { get; set; }

        [DataMember]
        public string Method { get; set; }
    }
}