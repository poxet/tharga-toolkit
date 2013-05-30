using System;
using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public class StopSubscriptionRequest
    {
        [DataMember]
        public Guid SubscriptionToken { get; set; }
    }
}