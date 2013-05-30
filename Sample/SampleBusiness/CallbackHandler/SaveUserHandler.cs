using System;
using SampleBusiness.Entities;
using SampleDataTransfer.Entities;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleBusiness.CallbackHandler
{
    public class SaveUserHandler : ISavedHandler<UserDto>
    {
        public void Handle(Guid userId, UserDto dto, DateTime? previousServerStoreTime)
        {
            SubscriptionCallbackBase.OnSaved(userId, dto, UserEntity.Convert, previousServerStoreTime);
        }
    }
}