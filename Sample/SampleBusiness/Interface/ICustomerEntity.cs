using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.Storage;

namespace SampleBusiness.Interface
{
    [MongoDBCollection(Name = "Customer")]
    public interface ICustomerEntity : IEntityWithValidation
    {
        string Name { get; set; }
        string Address { get; set; }
    }
}