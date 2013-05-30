using System.Runtime.Serialization;

namespace SampleDataTransfer.Entities
{
    [DataContract]
    public class MoveCustomerResponse
    {
        [DataMember]
        public string Message { get; set; }
    }
}