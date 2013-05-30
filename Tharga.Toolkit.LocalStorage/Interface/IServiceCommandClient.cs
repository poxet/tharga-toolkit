using System;

namespace Tharga.Toolkit.LocalStorage.Interface
{
    public interface IServiceCommandClient
    {
        void CheckSubscription(Guid subscriptionToken);
    }
}