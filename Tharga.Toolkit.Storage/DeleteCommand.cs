using System;
using System.Runtime.Serialization;

namespace Tharga.Toolkit.Storage
{
    [DataContract]
    public class DeleteCommand : ICommand
    {
        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public Guid Id { get; set; }
    }
}