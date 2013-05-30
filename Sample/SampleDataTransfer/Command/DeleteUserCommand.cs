using System;
using System.Runtime.Serialization;

namespace SampleDataTransfer.Command
{
    [DataContract]
    public class DeleteUserCommand
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}