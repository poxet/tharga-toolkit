using System;
using SampleBusiness;
using SampleBusiness.Business;
using SampleBusiness.Entities;
using SampleBusiness.Interface;
using SampleBusiness.Repository.Local;
using SampleBusiness.Repository.Service;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient
{
    public class AggregateRoot
    {
        private static readonly Lazy<AggregateRoot> InstanceLoader = new Lazy<AggregateRoot>(() => new AggregateRoot());
        private readonly Lazy<ISubscriptionHandler> _subscriptionHandlerLoader = new Lazy<ISubscriptionHandler>(() => new SubscriptionHandler(new SubscriptionServiceRepository()));
        private readonly Lazy<ProductBusiness> _productBusinessLoader = new Lazy<ProductBusiness>(() => new ProductBusiness(new ProductLocalRepository(), new ProductServiceRepository(), Instance.SubscriptionHandler));
        private readonly Lazy<GenericRealmBusiness<ICustomerEntity>> _customerBusinessLoader = new Lazy<GenericRealmBusiness<ICustomerEntity>>(() => new GenericRealmBusiness<ICustomerEntity>(SampleBusiness.Converter.Converter.ToCustomerDto, Instance.SubscriptionHandler, typeof(CustomerEntity)));
        private readonly Lazy<UserBusiness> _userBusinessLoader = new Lazy<UserBusiness>(() => new UserBusiness(new UserLocalRepository(), new UserServiceRepository(), Instance.SubscriptionHandler));
        private readonly Lazy<RealmBusiness> _realmBusiness = new Lazy<RealmBusiness>(() => new RealmBusiness(new RealmLocalRepository(), new RealmServiceRepository(), Instance.SubscriptionHandler));

        public static AggregateRoot Instance { get { return InstanceLoader.Value; } }

        public ISubscriptionHandler SubscriptionHandler { get { return _subscriptionHandlerLoader.Value; } }
        public ProductBusiness ProductBusiness { get { return _productBusinessLoader.Value; } }
        public GenericRealmBusiness<ICustomerEntity> CustomerBusiness { get { return _customerBusinessLoader.Value; } }
        public UserBusiness UserBusiness { get { return _userBusinessLoader.Value; } }
        public RealmBusiness RealmBusiness { get { return _realmBusiness.Value; } }

        private AggregateRoot()
        {
            
        }
    }
}