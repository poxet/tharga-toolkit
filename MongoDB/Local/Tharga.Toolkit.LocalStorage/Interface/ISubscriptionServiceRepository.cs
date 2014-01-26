using System;
using Tharga.Toolkit.LocalStorage.Entity;

namespace Tharga.Toolkit.LocalStorage.Interface
{
    public interface ISubscriptionServiceRepository
    {
        event EventHandler<SubscriberChangeEventArgs> SubsrciberChangeEvent;
        event EventHandler<SubscriptionStartedEventArgs> SubscriptionStartedEvent;
        event EventHandler<SubscriptionCheckedEventArgs> SubscriptionCheckedEvent;
        event EventHandler<SubscriptionStoppedEventArgs> SubscriptionStoppedEvent;
        event EventHandler<SessionCreatedEventArgs> SessionCreatedEvent;
        event EventHandler<SessionEndedEventArgs> SessionEndedEvent;
        void StartSubscription();
        void StopSubscription();
        void CheckSubscriptionByMessage();
        void CheckSubscriptionByCommand();
        void CreateSession(string userName, string password);
        void EndSession();
        void Execute(object command);
    }
}