using System;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Entity;

namespace Tharga.Toolkit.StorageConsole.Command
{
    public class RootCommand : RootCommandBase 
    {
        private readonly string _outgoingCommandQueueName;
        private readonly SubscriptionHandler _subscriptionHandler;

        public RootCommand(SubscriptionHandler subscriptionHandler, string outgoingCommandQueueName)
            : this(new ClientConsole(), null, subscriptionHandler, outgoingCommandQueueName)
        {

        }

        private RootCommand(IConsole console, Action stopAction, SubscriptionHandler subscriptionHandler, string outgoingCommandQueueName)
            : base(console, stopAction)
        {
            _outgoingCommandQueueName = outgoingCommandQueueName;

            _subscriptionHandler = subscriptionHandler;
            _subscriptionHandler.SubscriptionStartedEvent += SubscriptionHandler_SubscriptionStartedEvent;
            _subscriptionHandler.SubscriptionStoppedEvent += SubscriptionHandler_SubscriptionStoppedEvent;
            _subscriptionHandler.SubscriptionCheckedEvent += SubscriptionHandler_SubscriptionCheckedEvent;
            _subscriptionHandler.SubscriberChangeEvent += SubscriptionHandlerSubscriberChangeEvent;
            _subscriptionHandler.CheckOnlineStatusEvent += Instance_CheckOnlineStatusEvent;
            _subscriptionHandler.SessionCreatedEvent += _subscriptionHandler_SessionCreatedEvent;
            _subscriptionHandler.SessionCreatedFailedEvent += _subscriptionHandler_SessionCreatedFailedEvent;
            _subscriptionHandler.SessionEndedEvent +=_subscriptionHandler_SessionEndedEvent;

            UnregisterCommand("Exit");
            RegisterCommand(new ExitCommand(console, stopAction, subscriptionHandler));
            RegisterCommand(new SubscriptionCommand(console, subscriptionHandler));
            RegisterCommand(new QueueCommand(console, _outgoingCommandQueueName));
            RegisterCommand(new UserCommand(console, subscriptionHandler));

            _subscriptionHandler.StartSubscriptionAsync();
        }

        protected internal override void SetStopAction(Action stopAction)
        {
            var exitCommand = GetCommand("exit");

            if (exitCommand is ExitCommand)
                ((ExitCommand)GetCommand("exit")).SetStopAction(stopAction);
            else
                base.SetStopAction(stopAction);
        }

        void SubscriptionHandler_SubscriptionStartedEvent(object sender, SubscriptionStartedEventArgs e)
        {
            OutputEvent("Subscription to {0} started by client {1}.", e.ServerAddress, e.ClientAddress);
        }

        private void SubscriptionHandler_SubscriptionStoppedEvent(object sender, SubscriptionStoppedEventArgs e)
        {
            OutputEvent("Subscription to {0} stopped by {1}. {2}", e.ServerAddress, e.ClientAddress, e.Reason);
        }

        void SubscriptionHandler_SubscriptionCheckedEvent(object sender, SubscriptionCheckedEventArgs e)
        {
            OutputEvent("Subscription to {0} checked by {1}. {2}", e.ServerAddress, e.Method, e.Restarted ? "Subscription was restarted." : "Subscription is working.");
        }

        void SubscriptionHandlerSubscriberChangeEvent(object sender, SubscriberChangeEventArgs e)
        {
            OutputEvent("Subscriber to {0} attached or detached. Now {1} subscribers.", e.ServerAddress, e.SubscriberCount);
        }

        void Instance_CheckOnlineStatusEvent(object sender, CheckOnlineStatusEventArgs e)
        {
            OutputEvent("Checked if the server {0} could be reached. Checked {1} times with no luck.", e.ServerAddress, e.RetryCount);
        }

        void _subscriptionHandler_SessionCreatedEvent(object sender, SessionCreatedEventArgs e)
        {
            OutputEvent("Session {0} for realm {1} was created.", e.SessionToken, e.RealmId);
        }

        void _subscriptionHandler_SessionCreatedFailedEvent(object sender, SessionCreatedFailedEventArgs e)
        {
            OutputEvent("Session could not be created.");
        }

        void _subscriptionHandler_SessionEndedEvent(object sender, SessionEndedEventArgs e)
        {
            OutputEvent("Session {0} has ended.", e.SessionToken);
        }
    }
}
