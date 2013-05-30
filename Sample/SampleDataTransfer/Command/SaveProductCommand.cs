using System.Runtime.Serialization;
using SampleDataTransfer.Entities;

namespace SampleDataTransfer.Command
{
    [DataContract]
    public class SaveProductCommand
    {
        [DataMember]
        public ProductDto Product { get; set; }

        [DataMember]
        public bool NotifySubscribers { get; set; }
    }
}