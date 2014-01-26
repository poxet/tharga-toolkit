using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    [ServiceKnownType("GetKnownTypes", typeof(KnownEntityTypesProvider))]
    public class OnSavedResponse : IOnSavedRequest
    {
        [DataMember]
        public object Entity { get; set; }

        [DataMember]
        public Guid RealmId { get; set; }

        [DataMember]
        public DateTime? PreviousServerStoreTime { get; set; }
    }
}