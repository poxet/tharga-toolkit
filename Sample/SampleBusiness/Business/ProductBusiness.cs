using SampleBusiness.Interface;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleBusiness.Business
{
    public class ProductBusiness : BusinessBase<IProductEntity>
    {
        public ProductBusiness(ILocalRepository<IProductEntity> localRepository, IServiceRepository<IProductEntity> serviceRepository, ISubscriptionHandler subscriptionHandler)
            : base(localRepository, serviceRepository, subscriptionHandler)
        {

        }
    }
}