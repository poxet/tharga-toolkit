using System;
using Tharga.Toolkit.LocalStorage.Entity;

namespace Tharga.Toolkit.LocalStorage.Interface
{
    public interface ISubscriptionServiceClient
    {
        void Stop(SubscriptionStoppedEventArgs.StopReason reason, string clientAddress, string serverAddress);

        event EventHandler<SubscriptionStartedEventArgs> SubscriptionStartedEvent;
        event EventHandler<SubscriptionStoppedEventArgs> SubscriptionStoppedEvent;
        event EventHandler<SubscriptionCheckedEventArgs> SubscriptionCheckedEvent;
        event EventHandler<SubscriberChangeEventArgs> SubscriberChangeEvent;
        event EventHandler<SessionCreatedEventArgs> SessionCreatedEvent;
        event EventHandler<SessionEndedEventArgs> SessionEndedEvent;
        event EventHandler<ExecuteCompleteEventArgs> ExecuteCompleteEvent;
    }
}