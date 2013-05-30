using SampleDataTransfer.Entities;
using Tharga.Toolkit.ServerStorage;

namespace SampleConsoleHost.Business
{
    public class ProductBusiness : BusinessBase<ProductDto, ProductDto>
    {
        public ProductBusiness()
        {
            //TODO: Have the same Unique Index rules in the server and in the local dabase
            RepositoryInstance.EnsureUniqueIndex(new[] { "Name" });
        }

        protected override ProductDto Convert(ProductDto input)
        {
            return input;
        }
    }
}