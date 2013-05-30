using SampleBusiness.Business;
using SampleBusiness.Interface;

namespace SampleWpfClient.Model
{
    class ProductModel : ModelBase<IProductEntity>
    {
        public ProductModel(ProductBusiness business)
            : base(business,true)
        {

        }
    }
}
