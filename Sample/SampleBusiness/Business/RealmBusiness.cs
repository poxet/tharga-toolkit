using SampleBusiness.Interface;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleBusiness.Business
{
    public class RealmBusiness :BusinessBase<IRealmEntity>
    {
        public RealmBusiness(ILocalRepository<IRealmEntity> localRepository, IServiceRepository<IRealmEntity> serviceRepository, ISubscriptionHandler subscriptionHandler) 
            : base(localRepository, serviceRepository, subscriptionHandler)
        {

        }
    }
}