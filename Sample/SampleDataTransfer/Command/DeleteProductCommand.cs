using System;
using System.Runtime.Serialization;

namespace SampleDataTransfer.Command
{
    [DataContract]
    public class DeleteProductCommand
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}