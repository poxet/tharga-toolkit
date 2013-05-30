using System;
using SampleBusiness.Entities;
using SampleDataTransfer.Entities;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleBusiness.CallbackHandler
{
    public class DeleteProductHandler : IDeleteHandler<ProductDto>
    {
        public void Handle(Guid realmId, ProductDto dto, DateTime? previousServerStoreTime)
        {
            SubscriptionCallbackBase.OnDeleted(realmId, dto, ProductEntity.Convert, previousServerStoreTime);
        }
    }
}