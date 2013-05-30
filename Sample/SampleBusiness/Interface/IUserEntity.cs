using System;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.Storage;

namespace SampleBusiness.Interface
{
    [MongoDBCollection(Name = "User")]
    public interface IUserEntity : IEntity
    {
        string UserName { get; }
        string PasswordHash { get; }
        Guid RealmId { get; }
    }
}