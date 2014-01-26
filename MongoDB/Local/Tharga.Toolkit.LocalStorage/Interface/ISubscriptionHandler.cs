using System;
using System.Threading.Tasks;
using Tharga.Toolkit.LocalStorage.Entity;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.LocalStorage.Interface
{
    public interface ISubscriptionHandler
    {
        bool IsOnline { get; }
        void GoOffline();

        ISession Session { get; }

        Task StartSubscriptionAsync();
        Task StopSubscriptionAsync();
        Task CheckSubscriptionAsync();
        Task ExecuteAsync(object command);
        Task CreateSession(string userName, string password);
        Task EndSession();

        event EventHandler<SubscriberChangeEventArgs> SubscriberChangeEvent;
        event EventHandler<SubscriptionStartedEventArgs> SubscriptionStartedEvent;
        event EventHandler<SubscriptionStoppedEventArgs> SubscriptionStoppedEvent;
        event EventHandler<SubscriptionCheckedEventArgs> SubscriptionCheckedEvent;
        event EventHandler<CheckOnlineStatusEventArgs> CheckOnlineStatusEvent;
        event EventHandler<SessionCreatedEventArgs> SessionCreatedEvent;
        event EventHandler<SessionCreatedFailedEventArgs> SessionCreatedFailedEvent;
        event EventHandler<SessionEndedEventArgs> SessionEndedEvent;
    }
}