using MongoDB.Bson.Serialization;
using SampleBusiness.Entities;
using SampleBusiness.Interface;
using Tharga.Toolkit.LocalStorage.Repository;

namespace SampleBusiness.Repository.Local
{
    public class UserLocalRepository : LocalRepositoryBase<IUserEntity>
    {
        static UserLocalRepository()
        {
            BsonClassMap.RegisterClassMap<UserEntity>();
        }
    }
}