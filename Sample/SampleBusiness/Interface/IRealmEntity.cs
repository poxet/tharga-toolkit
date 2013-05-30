using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.Storage;

namespace SampleBusiness.Interface
{
    [MongoDBCollection(Name = "Realm")]
    public interface IRealmEntity : IEntity
    {
        string Name { get; }
    }
}