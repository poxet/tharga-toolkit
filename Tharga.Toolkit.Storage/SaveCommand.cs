using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public class SaveCommand : ICommand
    {
        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public object Item { get; set; }

        [DataMember]
        public bool NotifySubscribers { get; set; }
    }
}