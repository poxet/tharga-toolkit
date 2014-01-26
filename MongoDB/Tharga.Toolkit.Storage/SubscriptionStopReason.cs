using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public enum SubscriptionStopReason
    {
        [EnumMember]
        RequestedByClient,

        [EnumMember]
        RequestedByServer,
    };
}