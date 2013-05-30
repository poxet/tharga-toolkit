//using System;
//using SampleBusiness.Entities;
//using SampleDataTransfer.Entities;
//using Tharga.Toolkit.LocalStorage.Business;
//using Tharga.Toolkit.LocalStorage.Interface;

//namespace SampleBusiness.CallbackHandler
//{
//    public class DeleteCustomerHandler : IDeleteHandler<CustomerDto>
//    {
//        public void Handle(Guid realmId, CustomerDto dto, DateTime? previousServerStoreTime)
//        {
//            SubscriptionCallbackBase.OnDeleted(realmId, dto, CustomerEntity.Convert, previousServerStoreTime);
//        }
//    }
//}