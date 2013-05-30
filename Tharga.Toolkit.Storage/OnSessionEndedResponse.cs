using System;
using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public class OnSessionEndedResponse
    {
        [DataMember]
        public Guid SessionToken { get; set; }
    }
}