using System;
using System.Collections.Generic;
using System.Linq;
using SampleBusiness.Entities;
using SampleDataTransfer.Entities;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleBusiness.CallbackHandler
{
    public class SyncUserHandler : ISyncHandler<UserDto>
    {
        public void Handle(SubscriptionCallbackBase sc, Guid realmId, List<object> changed, List<object> deleted)
        {
            sc.OnSync(realmId, changed.Select(x => x as UserDto), deleted.Select(x => x as UserDto), UserEntity.Convert);
        }
    }
}