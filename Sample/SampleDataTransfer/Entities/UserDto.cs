using System;
using System.Runtime.Serialization;
using Tharga.Toolkit.ServerStorage.DataTransfer;
using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.Storage;

namespace SampleDataTransfer.Entities
{
    [DataContract]
    [MongoDBCollection(Name = "Users")]
    public class UserDto : IOutputDto, IInputDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }

        [DataMember]
        public Guid RealmId { get; set; }

        [DataMember]
        public StoreInfoDto StoreInfo { get; set; }        
    }
}