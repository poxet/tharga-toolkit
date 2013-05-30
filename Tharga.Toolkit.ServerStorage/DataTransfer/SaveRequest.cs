using System.Runtime.Serialization;

namespace Tharga.Toolkit.ServerStorage.DataTransfer
{
    [DataContract]
    public class SaveRequest
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Entity { get; set; }

        [DataMember]
        public bool NotifySubscribers { get; set; }
    }
}