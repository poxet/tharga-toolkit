using System;
using System.Runtime.Serialization;

namespace SampleDataTransfer.Command
{
    [DataContract]
    public class MoveCustomerCommand
    {
        [DataMember]
        public Guid CustomerId { get; set; }

        [DataMember]
        public string NewAddress { get; set; }        
    }
}