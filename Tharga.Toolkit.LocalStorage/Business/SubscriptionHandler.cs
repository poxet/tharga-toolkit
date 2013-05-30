using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using Tharga.Toolkit.LocalStorage.Entity;
using Tharga.Toolkit.LocalStorage.Exceptions;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.LocalStorage.Business
{
    public class SubscriptionHandler : ISubscriptionHandler
    {
        private readonly Timer _checkOnlineStatusTimer = new Timer();
        private readonly ISubscriptionServiceRepository _subscriptionServiceRepository;

        private bool _eventListenersRegistered;
        private int _retryCounter;

        public bool IsOnline { get; private set; }
        public ISession Session { get; private set; }

        #region Event


        public event EventHandler<SubscriberChangeEventArgs> SubscriberChangeEvent;
        public event EventHandler<SubscriptionStartedEventArgs> SubscriptionStartedEvent;
        public event EventHandler<SubscriptionStoppedEventArgs> SubscriptionStoppedEvent;
        public event EventHandler<SubscriptionCheckedEventArgs> SubscriptionCheckedEvent;
        public event EventHandler<CheckOnlineStatusEventArgs> CheckOnlineStatusEvent;
        public event EventHandler<SessionCreatedEventArgs> SessionCreatedEvent;
        public event EventHandler<SessionCreatedFailedEventArgs> SessionCreatedFailedEvent;
        public event EventHandler<SessionEndedEventArgs> SessionEndedEvent;

        private void InvokeSubscriberChangeEvent(SubscriberChangeEventArgs e)
        {
            var handler = SubscriberChangeEvent;
            if (handler != null)
                handler(this, e);
        }

        private void InvokeSubscriptionStartedEvent(SubscriptionStartedEventArgs e)
        {
            var handler = SubscriptionStartedEvent;
            if (handler != null)
                handler(this, e);
        }

        private void InvokeSubscriptionStoppedEvent(SubscriptionStoppedEventArgs e)
        {
            var handler = SubscriptionStoppedEvent;
            if (handler != null)
                handler(this, e);
        }

        private void InvokeSubscriptionCheckedEvent(SubscriptionCheckedEventArgs e)
        {
            var handler = SubscriptionCheckedEvent;
            if (handler != null)
                handler(this, e);
        }

        private void InvokeCheckOnlineStatusEvent(CheckOnlineStatusEventArgs e)
        {
            var handler = CheckOnlineStatusEvent;
            if (handler != null)
                handler(this, e);
        }

        private void InvokeSessionCreatedEvent(SessionCreatedEventArgs e)
        {
            var handler = SessionCreatedEvent;
            if (handler != null) handler(this, e);
        }

        private void InvokeSessionCreatedFailedEvent(SessionCreatedFailedEventArgs e)
        {
            var handler = SessionCreatedFailedEvent;
            if (handler != null) handler(this, e);
        }

        private void InvokeSessionEndedEvent(SessionEndedEventArgs e)
        {
            var handler = SessionEndedEvent;
            if (handler != null) handler(this, e);
        }


        #endregion

        public SubscriptionHandler(ISubscriptionServiceRepository subscriptionServiceRepository)
        {
            _subscriptionServiceRepository = subscriptionServiceRepository;
        }

        private void _subscriptionServiceRepository_SubscriberChangeEvent(object sender, SubscriberChangeEventArgs e)
        {
            InvokeSubscriberChangeEvent(e);
        }

        public async Task ExecuteAsync(object command)
        {
            var task = Task.Factory.StartNew(() => _subscriptionServiceRepository.Execute(command));
            await task;
        }

        private async Task StartSubscriptionEx()
        {
            if (_subscriptionServiceRepository == null) throw new NoSubscriptionRepositoryAssignedException();

            if (!_eventListenersRegistered)
            {
                _subscriptionServiceRepository.SubsrciberChangeEvent += _subscriptionServiceRepository_SubscriberChangeEvent;
                _subscriptionServiceRepository.SubscriptionStartedEvent += _subscriptionServiceRepository_SubscriptionStartedEvent;
                _subscriptionServiceRepository.SubscriptionCheckedEvent += _subscriptionServiceRepository_SubscriptionCheckedEvent;
                _subscriptionServiceRepository.SubscriptionStoppedEvent += _subscriptionServiceRepository_SubscriptionStoppedEvent;
                _subscriptionServiceRepository.SessionCreatedEvent += _subscriptionServiceRepository_SessionCreatedEvent;
                _subscriptionServiceRepository.SessionEndedEvent += _subscriptionServiceRepository_SessionEndedEvent;
                _eventListenersRegistered = true;
            }
            var task = Task.Factory.StartNew(_subscriptionServiceRepository.StartSubscription);
            await task;
        }

        private void _subscriptionServiceRepository_SubscriptionStoppedEvent(object sender, SubscriptionStoppedEventArgs e)
        {
            GoOfflineEx(e.Reason, e.ClientAddress, e.ServerAddress);
        }

        void _subscriptionServiceRepository_SessionCreatedEvent(object sender, SessionCreatedEventArgs e)
        {
            if (e.SessionToken != Guid.Empty)
            {
                Session = new Session(e.SessionToken, e.RealmId);
                InvokeSessionCreatedEvent(e);
            }
            else
            {
                Session = null;
                InvokeSessionCreatedFailedEvent(new SessionCreatedFailedEventArgs());
            }
        }

        void _subscriptionServiceRepository_SessionEndedEvent(object sender, SessionEndedEventArgs e)
        {
            Session = null;
            InvokeSessionEndedEvent(e);
        }

        private void _subscriptionServiceRepository_SubscriptionStartedEvent(object sender, SubscriptionStartedEventArgs e)
        {
            IsOnline = true;
            InvokeSubscriptionStartedEvent(e);
        }

        private void _subscriptionServiceRepository_SubscriptionCheckedEvent(object sender, SubscriptionCheckedEventArgs e)
        {
            InvokeSubscriptionCheckedEvent(e);
        }

        private void GoOfflineEx(SubscriptionStoppedEventArgs.StopReason reason, string clientAddress, string serverAddress)
        {
            if (!IsOnline) return;

            IsOnline = false;
            InvokeSubscriptionStoppedEvent(new SubscriptionStoppedEventArgs(reason, clientAddress, serverAddress));

            BusinessBaseHelper.ClearAllSynced();

            if (reason == SubscriptionStoppedEventArgs.StopReason.TriggeredByServer || reason == SubscriptionStoppedEventArgs.StopReason.ConnectionLost)
                StartLookingForServer();
        }

        private void StartLookingForServer()
        {
            lock (_checkOnlineStatusTimer)
            {
                if (_checkOnlineStatusTimer.Enabled)
                    return;

                _retryCounter = 0;
                _checkOnlineStatusTimer.Interval = Helper.Settings.GetSetting("CheckOnlineStatusInterval", 15) * 1000;
                _checkOnlineStatusTimer.Elapsed += _checkOnlineStatusTimer_Elapsed;
                _checkOnlineStatusTimer.Start();
            }
        }

        private async void _checkOnlineStatusTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _checkOnlineStatusTimer.Stop();
            if (IsOnline) return;

            try
            {
                await StartSubscriptionEx();
            }
            catch (EndpointNotFoundException exception)
            {
                var serverUri = exception.Data["ServerAddress"] as Uri;
                var serverAddress = serverUri != null ? serverUri.ToString() : "N/A";

                InvokeCheckOnlineStatusEvent(new CheckOnlineStatusEventArgs(++_retryCounter, "Unknown_po1", serverAddress));
                _checkOnlineStatusTimer.Start();
            }
            catch (TimeoutException exception)
            {
                var serverUri = exception.Data["ServerAddress"] as Uri;
                var serverAddress = serverUri != null ? serverUri.ToString() : "N/A";

                InvokeCheckOnlineStatusEvent(new CheckOnlineStatusEventArgs(++_retryCounter, "Unknown_po3", serverAddress));
                _checkOnlineStatusTimer.Start();
            }
        }

        public void GoOffline()
        {
            GoOfflineEx(SubscriptionStoppedEventArgs.StopReason.ConnectionLost, "Unknown_po5", "Unknown_po6");
        }

        public async Task StopSubscriptionAsync()
        {
            if (!IsOnline) return;
            var task = Task.Factory.StartNew(_subscriptionServiceRepository.StopSubscription);
            await task;
        }

        public async Task StartSubscriptionAsync()
        {
            if (_subscriptionServiceRepository == null)
                throw new NoSubscriptionRepositoryAssignedException();

            await StartSubscriptionEx();
        }

        public async Task CheckSubscriptionAsync()
        {
            if (_subscriptionServiceRepository == null) throw new NoSubscriptionRepositoryAssignedException();

            var message = new Task(_subscriptionServiceRepository.CheckSubscriptionByMessage);
            var command = new Task(_subscriptionServiceRepository.CheckSubscriptionByCommand);

            message.Start();
            command.Start();

            await message;
            await command;
            //Task.WaitAll(message, command); //TODO CHECK: How does this wait work together with async?
        }

        public async Task CreateSession(string userName, string password)
        {
            //TODO: If there is an active session. End that session first

            var task = Task.Factory.StartNew(() => _subscriptionServiceRepository.CreateSession(userName, password));
            await task;
        }

        public async Task EndSession()
        {
            var task = Task.Factory.StartNew(() => _subscriptionServiceRepository.EndSession());
            await task;
        }
    }
}
