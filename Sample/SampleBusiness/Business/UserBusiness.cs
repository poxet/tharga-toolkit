using SampleBusiness.Interface;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleBusiness.Business
{
    public class UserBusiness : BusinessBase<IUserEntity>
    {
        public UserBusiness(ILocalRepository<IUserEntity> localRepository, IServiceRepository<IUserEntity> serviceRepository, ISubscriptionHandler subscriptionHandler) 
            : base(localRepository, serviceRepository, subscriptionHandler)
        {

        }
    }
}