using System;
using System.Collections.Generic;

namespace Tharga.Toolkit.ServerStorage.Interface
{
    public interface IServiceMessageBase
    {
        void NotifySyncComplete<TOutput>(Guid realmId, IEnumerable<TOutput> changed, IEnumerable<TOutput> deleted) where TOutput : new();
        void NotifySyncComplete(Guid realmId, IEnumerable<object> changed, IEnumerable<object> deleted, object defaultEntity);
        void NotifyExecuteComplete(object executeCompleteResponse);
    }
}