﻿using System;
using System.Runtime.Serialization;
using Tharga.Toolkit.ServerStorage.DataTransfer;
using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.Storage;

namespace SampleDataTransfer.Entities
{
    [DataContract]
    [MongoDBCollection(Name = "Customers")]
    public class CustomerDto : IOutputDto, IInputDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public StoreInfoDto StoreInfo { get; set; }
    }
}