using SampleBusiness.Converter;
using SampleBusiness.Interface;
using SampleDataTransfer.Entities;
using Tharga.Toolkit.LocalStorage.Entity;

namespace SampleBusiness.Entities
{
    public class ProductEntity : EntityBase, IProductEntity
    {
        public string Name { get; set; }

        internal static IProductEntity Convert(ProductDto arg)
        {
            return new ProductEntity
                {
                    Id = arg.Id,
                    Name = arg.Name,
                    StoreInfo = arg.StoreInfo.ToStoreInfo()
                };
        }
    }
}