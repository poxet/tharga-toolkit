using System;
using System.ServiceModel;
using System.Threading;
using Tharga.Toolkit.LocalStorage.Entity;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.LocalStorage.Repository;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.LocalStorage.Business
{
    public abstract class SubscriptionServiceRepositoryBase : ISubscriptionServiceRepository
    {
        private static readonly object SyncRoot = new object();

        private readonly AutoResetEvent _subscriptionStartedEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _subscriptionStoppedEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _subscriptionCheckedByMessageEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _subscriptionCheckedByCommandEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _sessionCreatedEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _sessionEndedEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _executeCompleteEvent = new AutoResetEvent(false);
        private ISubscriptionServiceClient _client;
        private Guid? _subscriptionToken;
        private Guid? _sessionToken;

        private readonly CallbackConnectionSignatures _callbackConnectionSignatures = new CallbackConnectionSignatures();

        protected abstract ISubscriptionServiceClient GetServiceClient();
        protected abstract IMessageServiceClient CreateEventServiceClient(InstanceContext callbackInstance);
        protected abstract IServiceCommandClient GetQueueClient();

        protected virtual void StartSubscription(IMessageServiceClient eventServiceClient)
        {
            eventServiceClient.StartSubscription(new StartSubscriptionRequest());
        }

        protected virtual void StopSubscription(IMessageServiceClient eventServiceClient, Guid subscriptionToken)
        {
            eventServiceClient.StopSubscription(new StopSubscriptionRequest { SubscriptionToken = subscriptionToken });
        }

        protected virtual void CheckSubscription(IMessageServiceClient eventServiceClient, Guid subscriptionToken)
        {
            eventServiceClient.CheckSubscription(new CheckSubscriptionRequest { SubscriptionToken = subscriptionToken });
        }

        protected virtual void CreateSession(IMessageServiceClient eventServiceClient, string userName, string password)
        {
            eventServiceClient.CreateSession(new CreateSessionRequest {UserName = userName, Password = password});
        }

        protected virtual void EndSession(IMessageServiceClient eventServiceClient, Guid sessionToken)
        {
            eventServiceClient.EndSession(new EndSessionRequest {SessionToken = sessionToken});
        }

        #region Event


        public event EventHandler<SubscriptionStartedEventArgs> SubscriptionStartedEvent;
        public event EventHandler<SubscriptionStoppedEventArgs> SubscriptionStoppedEvent;
        public event EventHandler<SubscriptionCheckedEventArgs> SubscriptionCheckedEvent;
        public event EventHandler<SubscriberChangeEventArgs> SubsrciberChangeEvent;
        public event EventHandler<SessionCreatedEventArgs> SessionCreatedEvent;
        public event EventHandler<SessionEndedEventArgs> SessionEndedEvent;


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

        private void InvokeSubscriberChangeEvent(SubscriberChangeEventArgs e)
        {
            var handler = SubsrciberChangeEvent;
            if (handler != null)
                handler(this, e);
        }

        private void InvokeSessionCreatedEvent(SessionCreatedEventArgs e)
        {
            var handler = SessionCreatedEvent;
            if (handler != null) handler(this, e);
        }

        private void InvokeSessionEndedEvent(SessionEndedEventArgs e)
        {
            var handler = SessionEndedEvent;
            if (handler != null) handler(this, e);
        }


        #endregion

        private ISubscriptionServiceClient Client
        {
            get
            {
                if (_client != null)
                    return _client;

                lock (SyncRoot)
                {
                    if (_client == null)
                    {
                        _client = GetServiceClient();

                        _client.SubscriptionStartedEvent += OnSubscriptionStartedEvent;
                        _client.SubscriptionStoppedEvent += OnSubscriptionStoppedEvent;
                        _client.SubscriptionCheckedEvent += OnSubscriptionCheckedEvent;
                        _client.SubscriberChangeEvent += OnSubscriberChangeEvent;
                        _client.SessionCreatedEvent += OnSessionCreatedEvent;
                        _client.SessionEndedEvent += OnSessionEndedEvent;
                        _client.ExecuteCompleteEvent += OnExecuteCompleteEvent;
                    }
                }
                return _client;
            }
        }

        private InstanceContext OpenSession(ISubscriptionServiceClient client)
        {
            var ccp = _callbackConnectionSignatures.Get(client, CreateEventServiceClient);
            Uri uri = null;
            try
            {
                var serviceEventClient = ccp.ServiceEventClient;
                uri = GetAddress(serviceEventClient); //TODO: Extract to method
                var state = serviceEventClient.State;

                if (state == CommunicationState.Faulted)
                    StartSubscription(serviceEventClient);
                else if (state == CommunicationState.Created || state == CommunicationState.Opened)
                    StartSubscription(serviceEventClient);
                else
                    throw new InvalidOperationException(string.Format("Unhandled state {0}.", state));
            }
            catch (ProtocolException)
            {
                ccp = RetryOpenSession(client);
            }
            catch (CommunicationObjectFaultedException)
            {
                ccp = RetryOpenSession(client);
            }
            //catch (EndpointNotFoundException exception)
            //{
            //    exception.Data.Add("ServerAddress", uri);
            //    throw;
            //}
            catch (Exception exception)
            {
                exception.Data.Add("ServerAddress", uri);
                throw;
            }
            
            return ccp.CallbackInstance;
        }

        private Uri GetAddress(IMessageServiceClient serviceEventClient)
        {
            return serviceEventClient.InnerChannel.RemoteAddress.Uri;
        }

        private CallbackConnectionSignature RetryOpenSession(ISubscriptionServiceClient client)
        {
            Uri uri = null;
            try
            {
                _callbackConnectionSignatures.Remove(client);
                var ccp = _callbackConnectionSignatures.Get(client, CreateEventServiceClient);
                uri = GetAddress(ccp.ServiceEventClient);
                StartSubscription(ccp.ServiceEventClient);
                return ccp;
            }
            catch (Exception exception)
            {                
                System.Diagnostics.Debug.WriteLine("Provide address {0} in response.", uri);
                throw;
            }
        }

        private void StopSubscription(ISubscriptionServiceClient client, Guid subscriptionToken)
        {
            var ccp = _callbackConnectionSignatures.Get(client, CreateEventServiceClient);
            StopSubscription(ccp.ServiceEventClient, subscriptionToken);
        }

        private void CheckSubscription(ISubscriptionServiceClient client, Guid subscriptionToken)
        {
            var ccp = _callbackConnectionSignatures.Get(client, CreateEventServiceClient);
            CheckSubscription(ccp.ServiceEventClient, subscriptionToken);
        }

        private void OnSubscriberChangeEvent(object sender, SubscriberChangeEventArgs e)
        {
            InvokeSubscriberChangeEvent(e);
        }

        private void OnSessionCreatedEvent(object sender, SessionCreatedEventArgs e)
        {
            if (e.SessionToken != Guid.Empty)
                _sessionToken = e.SessionToken;
            _sessionCreatedEvent.Set();
            InvokeSessionCreatedEvent(e);
        }

        private void OnSessionEndedEvent(object sender, SessionEndedEventArgs e)
        {
            _sessionToken = null;
            _sessionEndedEvent.Set();
            InvokeSessionEndedEvent(e);
        }

        private void OnSubscriptionStartedEvent(object sender, SubscriptionStartedEventArgs e)
        {
            _subscriptionToken = e.SubscriptionToken;
            _subscriptionStartedEvent.Set();
            InvokeSubscriptionStartedEvent(e);
        }

        private void OnSubscriptionStoppedEvent(object sender, SubscriptionStoppedEventArgs e)
        {
            if (_subscriptionToken == null) return;
            _subscriptionToken = null;
            _subscriptionStoppedEvent.Set();
            InvokeSubscriptionStoppedEvent(e);
        }

        private void OnExecuteCompleteEvent(object sender, ExecuteCompleteEventArgs e)
        {
            _executeCompleteEvent.Set();
        }

        private void OnSubscriptionCheckedEvent(object sender, SubscriptionCheckedEventArgs e)
        {
            switch (e.Method)
            {
                case SubscriptionCheckedEventArgs.EMethod.Command:
                    _subscriptionCheckedByCommandEvent.Set();
                    break;
                case SubscriptionCheckedEventArgs.EMethod.Message:
                    _subscriptionCheckedByMessageEvent.Set();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format("Unknown method {0}.", e.Method));
            }

            InvokeSubscriptionCheckedEvent(e);
        }

        public void StartSubscription()
        {
            if (_subscriptionToken != null)
                throw new InvalidOperationException(string.Format("Subscription is already started with token {0}", _subscriptionToken));

            RetryCall(() => OpenSession(Client));            

            var seconds = Helper.Settings.GetSetting("WcfTimeoutSeconds", ServiceRepositoryBase<IEntity>.DefaultWcfTimeoutSeconds);
            if (!_subscriptionStartedEvent.WaitOne(seconds * 1000))
                throw new TimeoutException(string.Format("Start subscription did not complete in {0} seconds.", seconds));
        }

        public void StopSubscription()
        {
            if (_subscriptionToken == null) throw new SystemException("Cannot stop subscription if there is no token.");

            RetryCall(() => StopSubscription(Client, _subscriptionToken.Value));

            var seconds = Helper.Settings.GetSetting("WcfTimeoutSeconds", ServiceRepositoryBase<IEntity>.DefaultWcfTimeoutSeconds);
            if (!_subscriptionStoppedEvent.WaitOne(seconds * 1000))
                throw new TimeoutException(string.Format("Stop subscription did not complete in {0} seconds.", seconds));
        }

        public void CheckSubscriptionByMessage()
        {
            if (_subscriptionToken == null) throw new SystemException("Cannot check subscription if there is no token.");

            RetryCall(() => CheckSubscription(Client, _subscriptionToken.Value));

            var seconds = Helper.Settings.GetSetting("WcfTimeoutSeconds", ServiceRepositoryBase<IEntity>.DefaultWcfTimeoutSeconds);
            if (!_subscriptionCheckedByMessageEvent.WaitOne(seconds * 1000))
                throw new TimeoutException(string.Format("Check subscription by message did not complete in {0} seconds.", seconds));
        }

        public void CheckSubscriptionByCommand()
        {
            if (_subscriptionToken == null) throw new SystemException("Cannot check subscription if there is no token.");

            var queue = GetQueueClient();
            queue.CheckSubscription(_subscriptionToken.Value);

            var seconds = Helper.Settings.GetSetting("WcfTimeoutSeconds", ServiceRepositoryBase<IEntity>.DefaultWcfTimeoutSeconds);
            if (!_subscriptionCheckedByCommandEvent.WaitOne(seconds * 1000))
                throw new TimeoutException(string.Format("Check subscription by command did not complete in {0} seconds.", seconds));
        }

        public void CreateSession(string userName, string password)
        {
            if (_sessionToken != null) throw new InvalidOperationException(string.Format("There is already an active session with token {0}", _sessionToken));
            //if (_subscriptionToken == null) throw new SystemException("Cannot create a session when there is no subscription.");

            RetryCall(() => CreateSession(Client, userName, password));

            var seconds = Helper.Settings.GetSetting("WcfTimeoutSeconds", ServiceRepositoryBase<IEntity>.DefaultWcfTimeoutSeconds);
            if (!_sessionCreatedEvent.WaitOne(seconds * 1000))
                throw new TimeoutException(string.Format("Create session did not complete in {0} seconds.", seconds));
        }

        private void CreateSession(ISubscriptionServiceClient client, string userName, string password)
        {
            var ccp = _callbackConnectionSignatures.Get(client, CreateEventServiceClient);
            CreateSession(ccp.ServiceEventClient, userName, password);
        }

        public void EndSession()
        {
            if (_sessionToken == null) throw new SystemException("Cannot end session if there is no token.");
            //if (_subscriptionToken == null) throw new SystemException("Cannot end a session when there is no subscription.");

            RetryCall(() => EndSession(Client, _sessionToken.Value));

            var seconds = Helper.Settings.GetSetting("WcfTimeoutSeconds", ServiceRepositoryBase<IEntity>.DefaultWcfTimeoutSeconds);
            if (!_sessionEndedEvent.WaitOne(seconds*1000))
            {
                _sessionToken = null; //Consider the session as lost, even though the server did not respond.
                throw new TimeoutException(string.Format("End session did not complete in {0} seconds.", seconds));
            }
        }

        private void EndSession(ISubscriptionServiceClient client, Guid sessionToken)
        {
            var ccp = _callbackConnectionSignatures.Get(client, CreateEventServiceClient);
            EndSession(ccp.ServiceEventClient, sessionToken);
        }

        public void Execute(object command)
        {
            lock (SyncRoot)
            {
                RetryCall(() =>
                    {
                        var ccp = _callbackConnectionSignatures.Get(Client, CreateEventServiceClient);
                        ccp.ServiceEventClient.Execute(_sessionToken ?? Guid.Empty, command);
                    });

                var seconds = Helper.Settings.GetSetting("WcfTimeoutSeconds", ServiceRepositoryBase<IEntity>.DefaultWcfTimeoutSeconds);
                if (!_executeCompleteEvent.WaitOne(seconds*1000))
                    throw new TimeoutException(string.Format("Execute command did not complete in {0} seconds.", seconds));
            }
        }

        private void RetryCall(Action call)
        {
            try
            {
                call();
            }
            catch (CommunicationObjectFaultedException exception)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", exception.Message);
                //Issue.Register(exception, false);

                //NOTE: This part is never tested, it should not occur. When the connection fails, the client should reset, so that it does not go into fault state.
                _client = null;
                call();
            }
        }
    }
}