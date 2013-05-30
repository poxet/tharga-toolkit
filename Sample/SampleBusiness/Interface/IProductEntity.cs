using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.Storage;

namespace SampleBusiness.Interface
{
    [MongoDBCollection(Name = "Product")]
    public interface IProductEntity : IEntity
    {
        string Name { get; set; }
    }
}