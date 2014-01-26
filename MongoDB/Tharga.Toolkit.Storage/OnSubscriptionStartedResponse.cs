using System;
using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public class OnSubscriptionStartedResponse
    {
        [DataMember]
        public Guid SubscriptionToken { get; set; }

        [DataMember]
        public string ClientAddress { get; set; }

        [DataMember]
        public string ServerAddress { get; set; }
    }
}