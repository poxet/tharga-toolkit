using System;
using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public class EndSessionRequest
    {
        [DataMember]
        public Guid SessionToken { get; set; }
    }
}