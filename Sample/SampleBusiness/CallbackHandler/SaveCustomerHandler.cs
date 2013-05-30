using System;
using SampleBusiness.Entities;
using SampleDataTransfer.Entities;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleBusiness.CallbackHandler
{
    public class SaveCustomerHandler : ISavedHandler<CustomerDto>
    {
        public void Handle(Guid realmId, CustomerDto dto, DateTime? previousServerStoreTime)
        {
            SubscriptionCallbackBase.OnSaved(realmId, dto, CustomerEntity.Convert, previousServerStoreTime);
        }
    }
}