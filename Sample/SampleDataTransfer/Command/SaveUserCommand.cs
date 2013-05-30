using System.Runtime.Serialization;
using SampleDataTransfer.Entities;

namespace SampleDataTransfer.Command
{
    [DataContract]
    public class SaveUserCommand
    {
        [DataMember]
        public UserDto User { get; set; }

        [DataMember]
        public bool NotifySubscribers { get; set; }
    }
}