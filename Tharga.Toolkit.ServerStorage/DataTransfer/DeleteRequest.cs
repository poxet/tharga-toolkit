using System;
using System.Runtime.Serialization;

namespace Tharga.Toolkit.ServerStorage.DataTransfer
{
    [DataContract]
    public class DeleteRequest
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}