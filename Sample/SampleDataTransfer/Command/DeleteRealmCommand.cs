using System;
using System.Runtime.Serialization;

namespace SampleDataTransfer.Command
{
    [DataContract]
    public class DeleteRealmCommand
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}