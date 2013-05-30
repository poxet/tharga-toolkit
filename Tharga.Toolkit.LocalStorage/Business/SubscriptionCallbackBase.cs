using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Toolkit.LocalStorage.Entity;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.LocalStorage.Repository;
using Tharga.Toolkit.LocalStorage.Utility;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.LocalStorage.Business
{
    public class SubscriptionCallbackBase : ISubscriptionServiceClient, ICallback
    {
        #region Event


        public event EventHandler<SubscriptionStartedEventArgs> SubscriptionStartedEvent;
        public event EventHandler<SubscriptionStoppedEventArgs> SubscriptionStoppedEvent;
        public event EventHandler<SubscriptionCheckedEventArgs> SubscriptionCheckedEvent;
        public event EventHandler<SubscriberChangeEventArgs> SubscriberChangeEvent;
        public event EventHandler<SessionCreatedEventArgs> SessionCreatedEvent;
        public event EventHandler<SessionEndedEventArgs> SessionEndedEvent;
        public event EventHandler<SyncCompleteData> SyncCompleteEvent;
        public event EventHandler<ExecuteCompleteEventArgs> ExecuteCompleteEvent;

        //public class SyncCompleteEventArgs : EventArgs
        //{
        //    public ISyncCompleteData SyncCompleteData { get; private set; }

        //    public SyncCompleteEventArgs(ISyncCompleteData syncCompleteData)
        //    {
        //        SyncCompleteData = syncCompleteData;
        //    }
        //}

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
            if (handler != null) handler(this, e);
        }

        private void InvokeSubscriberChangeEvent(SubscriberChangeEventArgs e)
        {
            var handler = SubscriberChangeEvent;
            if (handler != null)
                handler(this, e);
        }

        protected void InvokeSessionCreatedEvent(SessionCreatedEventArgs e)
        {
            var handler = SessionCreatedEvent;
            if (handler != null) handler(this, e);
        }

        protected void InvokeSessionEndedEvent(SessionEndedEventArgs e)
        {
            var handler = SessionEndedEvent;
            if (handler != null) handler(this, e);
        }

        private void InvokeSyncCompleteEvent(SyncCompleteData e)
        {
            var handler = SyncCompleteEvent;
            if (handler != null)
                handler(this, e);
        }

        private void InvokeExecuteCompleteEvent(ExecuteCompleteEventArgs e)
        {
            var handler = ExecuteCompleteEvent;
            if (handler != null) 
                handler(this, e);
        }


        #endregion

        public void Stop(SubscriptionStoppedEventArgs.StopReason reason, string clientAddress, string serverAddress)
        {
            InvokeSubscriptionStoppedEvent(new SubscriptionStoppedEventArgs(reason, clientAddress, serverAddress));
        }

        public void OnSubscriptionStarted(OnSubscriptionStartedResponse response)
        {
            TryExecute(() => InvokeSubscriptionStartedEvent(new SubscriptionStartedEventArgs(response.SubscriptionToken, response.ClientAddress, response.ServerAddress)));
        }

        public void OnSubscriptionStopped(OnSubscriptionStoppedResponse response)
        {
            TryExecute(() =>
            {
                SubscriptionStoppedEventArgs.StopReason reason;
                switch (response.Reason)
                {
                    case SubscriptionStopReason.RequestedByClient:
                        reason = SubscriptionStoppedEventArgs.StopReason.TriggeredByClient;
                        break;
                    case SubscriptionStopReason.RequestedByServer:
                        reason = SubscriptionStoppedEventArgs.StopReason.TriggeredByServer;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(string.Format("Unknown subscription stop reason {0}.", response.Reason));
                }

                Stop(reason, response.ClientAddress, response.ServerAddress);
            });
        }

        public void OnSubscriptionChecked(OnSubscriptionCheckedResponse response)
        {
            TryExecute(() =>
            {
                var method = (SubscriptionCheckedEventArgs.EMethod)Enum.Parse(typeof(SubscriptionCheckedEventArgs.EMethod), response.Method, true);
                InvokeSubscriptionCheckedEvent(new SubscriptionCheckedEventArgs(response.Restarted, response.ClientAddress, response.ServerAddress, method));
            });
        }

        public void OnSubscriberChange(OnSubscriberChangeResponse response)
        {
            TryExecute(() => InvokeSubscriberChangeEvent(new SubscriberChangeEventArgs(response.SubscriberCount, response.ClientAddress, response.ServerAddress)));
        }

        public void OnSessionCreated(OnSessionCreatedResponse response)
        {
            TryExecute(() => InvokeSessionCreatedEvent(new SessionCreatedEventArgs(response.SessionToken, response.RealmId)));
        }

        public void OnSessionEnded(OnSessionEndedResponse response)
        {
            TryExecute(() => InvokeSessionEndedEvent(new SessionEndedEventArgs(response.SessionToken)));
        }

        public void OnSync(OnSyncResponse response)
        {
            TryExecute(() =>
            {
                var commandHandlerType = typeof(ISyncHandler<>).MakeGenericType(response.Default.GetType());
                var commandHandler = Bootstrapper.GetInstance(commandHandlerType);
                commandHandlerType.GetMethod("Handle").Invoke(commandHandler, new object[] { this, response.RealmId, response.Changed, response.Deleted });
            });
        }

        public void OnSaved(OnSavedResponse response)
        {
            TryExecute(() =>
            {
                var commandHandlerType = typeof(ISavedHandler<>).MakeGenericType(response.Entity.GetType());
                var commandHandler = Bootstrapper.GetInstance(commandHandlerType);
                commandHandlerType.GetMethod("Handle").Invoke(commandHandler, new[] { response.RealmId, response.Entity, response.PreviousServerStoreTime });
            });
        }

        public void OnDeleted(OnDeletedResponse response)
        {
            TryExecute(() =>
            {
                var commandHandlerType = typeof(IDeleteHandler<>).MakeGenericType(response.Entity.GetType());
                var commandHandler = Bootstrapper.GetInstance(commandHandlerType);
                commandHandlerType.GetMethod("Handle").Invoke(commandHandler, new[] { response.RealmId, response.Entity, response.PreviousServerStoreTime });
            });
        }

        public void OnExecute(object response)
        {
            InvokeExecuteCompleteEvent(new ExecuteCompleteEventArgs(response));
        }

        public void OnSync<TEntity, TOutputDto>(Guid realmId, IEnumerable<TOutputDto> changed, IEnumerable<TOutputDto> deleted,
            Func<TOutputDto, TEntity> converter)
            where TEntity : IEntity
        {
            var list = changed.ToList();
            var deletedEntities = deleted.ToList();
            var lastServerStoreTime = (DateTime?) null;

            foreach (var item in list)
            {
                var entity = converter(item);
                ServiceRepositoryBase<TEntity>.InvokeEntityChangedOnSyncEvent(EntityChangedEventArgs<TEntity>.CreateServerOnSync(realmId, entity));
                if (lastServerStoreTime == null || entity.StoreInfo.ServerStoreTime > lastServerStoreTime)
                    lastServerStoreTime = entity.StoreInfo.ServerStoreTime;
            }

            foreach (var item in deletedEntities)
            {
                var entity = converter(item);
                ServiceRepositoryBase<TEntity>.InvokeEntityDeletedOnSyncEvent(EntityDeletedEventArgs<TEntity>.CreateServerOnSync(realmId, entity));
                if (lastServerStoreTime == null || entity.StoreInfo.ServerStoreTime > lastServerStoreTime)
                    lastServerStoreTime = entity.StoreInfo.ServerStoreTime;
            }

            InvokeSyncCompleteEvent(new SyncCompleteData(list.Count(), deletedEntities.Count(), lastServerStoreTime));
        }

        public static void OnSaved<TEntity, TOutputDto>(Guid realmId, TOutputDto item,
            Func<TOutputDto, TEntity> converter, DateTime? previousServerStoreTime)
            where TEntity : IEntity
        {
            ServiceRepositoryBase<TEntity>.InvokeEntityChangedEvent(EntityChangedEventArgs<TEntity>.CreateServer(realmId, converter(item), previousServerStoreTime));
        }

        public static void OnDeleted<TEntity, TOutputDto>(Guid realmId, TOutputDto item,
            Func<TOutputDto, TEntity> converter, DateTime? previousServerStoreTime)
            where TEntity : IEntity
        {
            ServiceRepositoryBase<TEntity>.InvokeEntityDeletedEvent(EntityDeletedEventArgs<TEntity>.CreateServer(realmId, converter(item), previousServerStoreTime));
        }

        protected void TryExecute(Action execute)
        {
            try
            {
                execute();
            }
            catch (Exception exception)
            {
                //Issue.Register(exception, false);
                System.Diagnostics.Debug.WriteLine("Swallowed message {0}", exception.Message);
            }
        }
    }
}