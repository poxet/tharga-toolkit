using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Tharga.Toolkit.ServerStorage.Utility;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.ServerStorage
{
    public abstract class ServiceMessageBase<TCallback>
    {
        private bool _closing;

        #region Event


        public class RequestEventArgs : EventArgs
        {
            public string Method { get; private set; }
            public TimeSpan Elapsed { get; private set; }
            public Exception Exception { get; private set; }
            public bool Success { get { return Exception == null; } }

            internal RequestEventArgs(string method, TimeSpan elapsed, Exception exception = null)
            {
                Method = method;
                Elapsed = elapsed;
                Exception = exception;
            }
        }

        public static event EventHandler<RequestEventArgs> RequestEvent;

        private static void InvokeRequestEvent(RequestEventArgs e)
        {
            var handler = RequestEvent;
            if (handler != null) handler(null, e);
        }


        #endregion

        protected static void ExecuteEvent(string method,Action block)
        {
            var sw = new StopWatch();

            try
            {
                block();

                InvokeRequestEvent(new RequestEventArgs(method,sw.Elapsed));
            }
            catch (Exception exception)
            {
                InvokeRequestEvent(new RequestEventArgs(method,sw.Elapsed, exception));
                throw;
            }
        }

        protected void OnSubscriptionStopped(Subscriber subscriber)
        {
            try
            {
                var response = new OnSubscriptionStoppedResponse
                    {
                        Reason = SubscriptionStopReason.RequestedByServer,
                        ClientAddress = GetClientCallerAddress(),
                        ServerAddress = GetServerAddress()
                    };

                subscriber.OnSubscriptionStopped(response);
            }
            catch (Exception exception)
            {
                //Issue.Register(exception);
                Debug.WriteLine("Swallowed Exception {0}", exception.Message);
            }
        }

        protected class Subscriber
        {
            public Guid SubscriptionToken { get; private set; }
            public Guid? RealmId { get; private set; }
            public TCallback Callback { get; private set; }
            public Action<OnSubscriptionStoppedResponse> OnSubscriptionStopped { get; private set; }
            public Action<OnSubscriberChangeResponse> OnSubscriberChange { get; private set; }

            public Subscriber(Guid subscriptionToken, TCallback callback, Action<OnSubscriptionStoppedResponse> onSubscriptionStopped,
                Action<OnSubscriberChangeResponse> subscriberChange)
            {
                SubscriptionToken = subscriptionToken;
                Callback = callback;
                OnSubscriptionStopped = onSubscriptionStopped;
                OnSubscriberChange = subscriberChange;
            }

            public void SetRealm(Guid realmId)
            {
                if (RealmId != null) throw new InvalidOperationException("The realm has already been set.");
                if (realmId == Guid.Empty) return;
                RealmId = realmId;
            }

            public void ClearRealm()
            {
                if (RealmId == null) throw new InvalidOperationException("There is no realm to clear.");

                RealmId = null;
            }
        }

        #region Event


        public class SubscriptionStartedEventArgs : EventArgs
        {
            public int SubscriberCount { get; private set; }

            internal SubscriptionStartedEventArgs(int subscriberCount)
            {
                SubscriberCount = subscriberCount;
            }
        }

        public class SubscriptionStoppedEventArgs : EventArgs
        {
            public int SubscriberCount { get; private set; }

            internal SubscriptionStoppedEventArgs(int subscriberCount)
            {
                SubscriberCount = subscriberCount;
            }
        }

        public class SubscriptionCheckedEventArgs : EventArgs
        {
            public int SubscriberCount { get; private set; }

            public SubscriptionCheckedEventArgs(int subscriberCount)
            {
                SubscriberCount = subscriberCount;
            }
        }

        public static event EventHandler<SubscriptionStartedEventArgs> SubscriptionStartedEvent;
        public static event EventHandler<SubscriptionStoppedEventArgs> SubscriptionStoppedEvent;
        public static event EventHandler<SubscriptionCheckedEventArgs> SubscriptionCheckedEvent;
        public static event EventHandler<EventArgs> ClientClosingEvent;
        public static event EventHandler<EventArgs> ClientClosedEvent;

        private static void InvokeSubscriptionStartedEvent(SubscriptionStartedEventArgs e)
        {
            var handler = SubscriptionStartedEvent;
            if (handler != null) handler(null, e);
        }

        private static void InvokeSubscriptionStoppedEvent(SubscriptionStoppedEventArgs e)
        {
            var handler = SubscriptionStoppedEvent;
            if (handler != null) handler(null, e);
        }

        private static void InvokeSubscriptionCheckedEvent(SubscriptionCheckedEventArgs e)
        {
            var handler = SubscriptionCheckedEvent;
            if (handler != null) handler(null, e);
        }

        private static void InvokeClientClosingEvent(EventArgs e)
        {
            var handler = ClientClosingEvent;
            if (handler != null)
                handler(null, e);
        }

        private static void InvokeClientClosedEvent(EventArgs e)
        {
            var handler = ClientClosedEvent;
            if (handler != null)
                handler(null, e);
        }


        #endregion

        protected static readonly List<Subscriber> Subscribers = new List<Subscriber>();        
        protected abstract TCallback GetCallbackChannel();
        public abstract void StartSubscription(StartSubscriptionRequest request);
        protected abstract void NotifySubscriptionChecked(Subscriber subscriber, string clientAddress, string serverAddress, bool restarted, string method);

        protected static IEnumerable<Subscriber> GetSubscribers(Guid realmId)
        {
            return Subscribers.Where(x => x.RealmId == realmId || realmId == Guid.Empty);
        }

        public virtual void CheckSubscription(CheckSubscriptionRequest request)
        {
            ExecuteEvent(request.GetType().Name, () =>
            {
                InvokeSubscriptionCheckedEvent(new SubscriptionCheckedEventArgs(Subscribers.Count));

                var subscription = Subscribers.FirstOrDefault(x => x.SubscriptionToken == request.SubscriptionToken);

                var restarted = false;
                if (subscription == null)
                {
                    if (_closing)
                        throw new CommunicationException("Server is shutting down.");

                    StartSubscription(new StartSubscriptionRequest());
                    //StartSubscriptionEx(request.SubscriptionToken,);
                    restarted = true;
                }

                var clientAddress = GetClientCallerAddress();
                var serverAddress = GetServerAddress();

                //Send message back to the caller.
                Parallel.ForEach(Subscribers.Where(x => x.SubscriptionToken == request.SubscriptionToken), merchantCallback => NotifySubscriptionChecked(merchantCallback, clientAddress, serverAddress, restarted, "Message"));

                //Send to all other subscribers as well, but not to the caller.
                Parallel.ForEach(Subscribers.Where(x => x.SubscriptionToken != request.SubscriptionToken), merchantCallback => NotifySubscriptionChecked(merchantCallback, clientAddress, serverAddress, false, "Message"));
            });
        }

        protected void StartSubscription(Guid subscriptionToken, Action<OnSubscriptionStartedResponse> onSubscriptionStarted, Action<OnSubscriptionStoppedResponse> onSubscriptionStopped, Action<OnSubscriberChangeResponse> subscriberChange)
        {
            if (_closing)
                throw new CommunicationException("Server is shutting down.");

            var callback = GetCallbackChannel();
            StartSubscriptionEx(subscriptionToken, callback, onSubscriptionStopped, subscriberChange);

            var response = new OnSubscriptionStartedResponse
                {
                    SubscriptionToken = subscriptionToken,
                    ClientAddress = GetClientCallerAddress(),
                    ServerAddress = GetServerAddress()
                };

            onSubscriptionStarted.Invoke(response);
            Parallel.ForEach(Subscribers, OnSubscriberChange);
        }

        protected static string GetClientCallerAddress()
        {
            var context = OperationContext.Current;

            if (context == null)
                return "N/A";

            var messageProperties = context.IncomingMessageProperties;
            if (!messageProperties.ContainsKey(RemoteEndpointMessageProperty.Name))
                return "N/A";

            var endpointProperty = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            if (endpointProperty == null) 
                return "Unknown";

            var clientAddress = string.Format("[{0}]:{1}", endpointProperty.Address, endpointProperty.Port);
            return clientAddress;
        }

        protected static string GetServerAddress()
        {
            var context = OperationContext.Current;
            if (context == null)
                return "N/A";

            return context.EndpointDispatcher.EndpointAddress.Uri.ToString();
        }

        private void StartSubscriptionEx(Guid subscriptionToken, TCallback callback, Action<OnSubscriptionStoppedResponse> onSubscriptionStopped,
            Action<OnSubscriberChangeResponse> subscriberChange)
        {
            var communicationObject = callback as ICommunicationObject;
            if (communicationObject != null)
            {
                communicationObject.Closing += EventService_Closing;
                communicationObject.Closed += EventService_Closed;
                communicationObject.Faulted += communicationObject_Faulted;
            }

            var subscriber = new Subscriber(subscriptionToken, callback, onSubscriptionStopped, subscriberChange);
            Subscribers.Add(subscriber);

            InvokeSubscriptionStartedEvent(new SubscriptionStartedEventArgs(Subscribers.Count));
        }

        public void StopSubscription(StopSubscriptionRequest request)
        {
            var subscriber = Subscribers.Single(x => x.SubscriptionToken == request.SubscriptionToken);
            subscriber.OnSubscriptionStopped.Invoke(new OnSubscriptionStoppedResponse());

            Subscribers.Remove(subscriber);
            var communicationObject = subscriber.Callback as ICommunicationObject;
            if (communicationObject != null)
                communicationObject.Close();

            //InvokeSubscriptionStoppedEvent(new SubscriptionStoppedEventArgs { SubscriberCount = Subscribers.Count });
        }

        private void EventService_Closing(object sender, EventArgs e)
        {
            //Remove the subscriber that is closing
            Subscribers.RemoveAll(x => ReferenceEquals(x.Callback, sender));

            Parallel.ForEach(Subscribers, OnSubscriberChange);
            InvokeSubscriptionStoppedEvent(new SubscriptionStoppedEventArgs(Subscribers.Count));
            InvokeClientClosingEvent(e);
        }

        protected void OnSubscriberChange(Subscriber subscriber)
        {
            try
            {
                subscriber.OnSubscriberChange(new OnSubscriberChangeResponse
                    {
                        SubscriberCount = Subscribers.Count, 
                        ClientAddress = GetClientCallerAddress(), 
                        ServerAddress = GetServerAddress()
                    });
            }
            catch (Exception exception)
            {
                //Issue.Register(exception);
                Debug.WriteLine("Swallowed Exception {0}", exception.Message);
            }
        }

        private void EventService_Closed(object sender, EventArgs e)
        {
            InvokeClientClosedEvent(e);
        }

        void communicationObject_Faulted(object sender, EventArgs e)
        {
            //Happens when a client closes without notice.
            Debug.WriteLine("communicationObject_Faulted");
        }
    }
}
