using System;
using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public class CheckSubscriptionRequest
    {
        [DataMember]
        public Guid SubscriptionToken { get; set; }
    }
}