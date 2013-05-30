using MongoDB.Bson.Serialization;
using SampleBusiness.Entities;
using SampleBusiness.Interface;
using Tharga.Toolkit.LocalStorage.Repository;

namespace SampleBusiness.Repository.Local
{
    public class RealmLocalRepository : LocalRepositoryBase<IRealmEntity>
    {
        static RealmLocalRepository()
        {
            BsonClassMap.RegisterClassMap<RealmEntity>();
        }
    }
}