using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.ServerStorage.Utility;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.ServerStorage
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public sealed class ServiceMessage : ServiceMessageBase<ICallback>, IServiceMessage, IServiceMessageBase
    {
        protected override ICallback GetCallbackChannel()
        {
            return OperationContectContainer<ICallback>.OperationContextRepository.GetCallbackChannel();
        }

        public override void StartSubscription(StartSubscriptionRequest request)
        {
            var callback = GetCallbackChannel();
            StartSubscription(Guid.NewGuid(), callback.OnSubscriptionStarted, callback.OnSubscriptionStopped, callback.OnSubscriberChange);
        }

        public static void NotifyAllSubscriptionChecked(string clientAddress, string serverAddress, string method)
        {
            clientAddress = clientAddress ?? GetClientCallerAddress();
            serverAddress = serverAddress ?? GetServerAddress();

            Parallel.ForEach(Subscribers, subscriber => NotifySubscriptionCheckedEx(subscriber, clientAddress, serverAddress, false, method));
        }

        public static void NotifyAllSaved<TOutput>(Guid realmId, TOutput output, DateTime? previousServerStoreTime)
        {
            Parallel.ForEach(GetSubscribers(realmId), subscriber => NotifySaved(realmId, output, previousServerStoreTime, subscriber));
        }

        public static void NotifyAllExecuteComplete(object executeCompleteResponse)
        {
            Parallel.ForEach(Subscribers, subscriber => NotifyExecuteComplete(executeCompleteResponse, subscriber));
        }

        public static void NotifyAllDeleted<TOutput>(Guid realmId, TOutput output, DateTime? previousServerStoreTime)
        {
            Parallel.ForEach(GetSubscribers(realmId), subscriber => NotifyDeleted(realmId, output, previousServerStoreTime, subscriber));
        }

        protected override void NotifySubscriptionChecked(Subscriber subscriber, string clientAddress, string serverAddress, bool restarted, string method)
        {
            NotifySubscriptionCheckedEx(subscriber, clientAddress, serverAddress, restarted, method);
        }

        private static void NotifySubscriptionCheckedEx(Subscriber subscriber, string clientAddress, string serverAddress, bool restarted = false, string method = "Command")
        {
            try
            {
                var response = new OnSubscriptionCheckedResponse
                    {
                        ClientAddress = clientAddress ?? GetClientCallerAddress(),
                        ServerAddress = serverAddress ?? GetServerAddress(),
                        Restarted = restarted, 
                        Method = method
                    };
                subscriber.Callback.OnSubscriptionChecked(response);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", exception.Message);
                Subscribers.RemoveAll(x => x.SubscriptionToken == subscriber.SubscriptionToken);
            }
        }

        private static void NotifySaved<TOutput>(Guid realmId, TOutput output, DateTime? previousServerStoreTime, Subscriber subscriber)
        {
            try
            {
                var callback = subscriber.Callback;
                var request = new OnSavedResponse
                    {
                        RealmId = realmId,
                        Entity = output,
                        PreviousServerStoreTime = previousServerStoreTime
                    };
                callback.OnSaved(request);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", exception.Message);
                Subscribers.RemoveAll(x => x.SubscriptionToken == subscriber.SubscriptionToken);
            }
        }

        private static void NotifyDeleted(Guid realmId, object output, DateTime? previousServerStoreTime, Subscriber subscriber)
        {
            try
            {
                var callback = subscriber.Callback;
                var request = new OnDeletedResponse
                    {
                        RealmId = realmId,
                        Entity = output, 
                        PreviousServerStoreTime = previousServerStoreTime
                    };
                callback.OnDeleted(request);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", exception.Message);
                Subscribers.RemoveAll(x => x.SubscriptionToken == subscriber.SubscriptionToken);
            }
        }

        public void CreateSession(CreateSessionRequest request)
        {
            ExecuteEvent("CreateSession", () =>
                {
                    var commandHandlerType = typeof(ICreateSessionHandler<>).MakeGenericType(request.GetType());
                    var commandHandler = Bootstrapper.GetInstance(commandHandlerType);
                    var response = (ISession) commandHandlerType.GetMethod("Handle").Invoke(commandHandler, new[] {request});

                    var callbackChannel = GetCallbackChannel();

                    if (response.SessionToken != Guid.Empty)
                    {
                        SessionRepository.Add(response);

                        //Decorate the subscriber with correct realm so that callbacks get sent there
                        var subscriber = Subscribers.Single(x => ReferenceEquals(x.Callback, callbackChannel));
                        subscriber.SetRealm(response.RealmId);
                    }
                    callbackChannel.OnSessionCreated(new OnSessionCreatedResponse { SessionToken = response.SessionToken, RealmId = response.RealmId });
                });
        }

        public void EndSession(EndSessionRequest request)
        {
            ExecuteEvent("EndSession", () =>
            {
                var commandHandlerType = typeof(IEndSessionHandler<>).MakeGenericType(request.GetType());
                var commandHandler = Bootstrapper.GetInstance(commandHandlerType);
                var response = (ISession)commandHandlerType.GetMethod("Handle").Invoke(commandHandler, new[] { request });

                //TODO: Mark the session as ended

                var callbackChannel = GetCallbackChannel();
                var subscriber = Subscribers.Single(x => ReferenceEquals(x.Callback, callbackChannel));
                subscriber.ClearRealm();

                callbackChannel.OnSessionEnded(new OnSessionEndedResponse { SessionToken = response.SessionToken });
            });
        }

        public void Execute(Guid sessionToken, object command)
        {
            ExecuteEvent(command.GetType().Name, () =>
                {                    
                    var commandHandlerType = typeof(IMessageHandler<>).MakeGenericType(command.GetType());
                    var commandHandler = Bootstrapper.GetInstance(commandHandlerType);

                    var realmId = Guid.Empty;
                    if (sessionToken != Guid.Empty)
                    {
                        var session = SessionRepository.Get(sessionToken);
                        realmId = session.RealmId;
                    }

                    commandHandlerType.GetMethod("Handle").Invoke(commandHandler, new[] { realmId, command, this });
                });
        }

        public void NotifySyncComplete(Guid realmId, IEnumerable<object> changed, IEnumerable<object> deleted, object defaultEntity)
        {
            var request = new OnSyncResponse
            {
                RealmId = realmId,
                Default = defaultEntity,
                Changed = changed.ToList(),
                Deleted = deleted.ToList()
            };
            GetCallbackChannel().OnSync(request);
        }

        public void NotifySyncComplete<TOutput>(Guid realmId, IEnumerable<TOutput> changed, IEnumerable<TOutput> deleted)
            where TOutput : new()
        {
            var request = new OnSyncResponse
                {
                    RealmId = realmId,
                    Default = new TOutput(),
                    Changed = changed.Select(x => (object)x).ToList(),
                    Deleted = deleted.Select(x => (object)x).ToList()
                };
            GetCallbackChannel().OnSync(request);
        }

        public void NotifyExecuteComplete(object executeCompleteResponse)
        {
            GetCallbackChannel().OnExecute(executeCompleteResponse);
            //NotifyExecuteComplete(executeCompleteResponse, GetCallbackChannel());
        }

        private static void NotifyExecuteComplete(object executeCompleteRequest, Subscriber subscriber)
        {
            try
            {
                var callback = subscriber.Callback;
                callback.OnExecute(executeCompleteRequest);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", exception.Message);
                Subscribers.RemoveAll(x => x.SubscriptionToken == subscriber.SubscriptionToken);
            }
        }
    }
}