using System;
using SampleBusiness.Entities;
using SampleDataTransfer.Entities;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleBusiness.CallbackHandler
{
    public class SaveRealmHandler : ISavedHandler<RealmDto>
    {
        public void Handle(Guid realmId, RealmDto dto, DateTime? previousServerStoreTime)
        {
            SubscriptionCallbackBase.OnSaved(realmId, dto, RealmEntity.Convert, previousServerStoreTime);
        }
    }
}