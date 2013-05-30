using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public class OnSubscriptionStoppedResponse
    {
        [DataMember]
        public SubscriptionStopReason Reason { get; set; }

        [DataMember]
        public string ClientAddress { get; set; }

        [DataMember]
        public string ServerAddress { get; set; }
    }
}