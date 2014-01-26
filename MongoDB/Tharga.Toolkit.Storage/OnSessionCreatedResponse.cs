using System;
using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public class OnSessionCreatedResponse
    {
        [DataMember]
        public Guid SessionToken { get; set; }

        [DataMember]
        public Guid RealmId { get; set; }
    }
}