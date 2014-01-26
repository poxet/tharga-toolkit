using System;
using System.Collections.Generic;
using Tharga.Toolkit.LocalStorage.Business;

namespace Tharga.Toolkit.LocalStorage.Interface
{
    public interface ISyncHandler<TDto>
    {
        void Handle(SubscriptionCallbackBase sc, Guid realmId, List<object> changed, List<object> deleted);
    }
}