using System;
using System.Collections.Generic;
using System.Linq;
using SampleBusiness.Entities;
using SampleDataTransfer.Entities;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleBusiness.CallbackHandler
{
    public class SyncCustomerHandler : ISyncHandler<CustomerDto>
    {
        public void Handle(SubscriptionCallbackBase sc, Guid realmId, List<object> changed, List<object> deleted)
        {
            sc.OnSync(realmId, changed.Select(x => x as CustomerDto), deleted.Select(x => x as CustomerDto), CustomerEntity.Convert);
        }
    }
}