using System;
using System.Diagnostics;
using System.Threading;
using Tharga.Toolkit.LocalStorage.Entity;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Repository
{
    public abstract class ServiceRepositoryBase<TEntity> : IServiceRepository<TEntity>
        where TEntity : IEntity
    {
        public const int DefaultWcfTimeoutSeconds = 30;

        private static ServiceRepositoryBase<TEntity> _instance;

        protected readonly AutoResetEvent SyncCompleteEvent = new AutoResetEvent(false);
        protected ISyncCompleteData SyncCompleteData;

        public ServiceRepositoryBase()
        {
            if (_instance != null)
                throw new InvalidOperationException(string.Format("ServiceRepositoryBase<{0}> has already been instantiated.", typeof(TEntity)));

            _instance = this;
        }

        #region Event

        public event EventHandler<EntityChangedEventArgs<TEntity>> EntityChangedEvent;
        public event EventHandler<EntityChangedEventArgs<TEntity>> EntityChangedOnSyncEvent;
        public event EventHandler<EntityDeletedEventArgs<TEntity>> EntityDeletedEvent;
        public event EventHandler<EntityDeletedEventArgs<TEntity>> EntityDeletedOnSyncEvent;

        public static void InvokeEntityChangedEvent(EntityChangedEventArgs<TEntity> e)
        {
            if (_instance == null) throw new InvalidOperationException(string.Format("ServiceRepositoryBase<{0}> has never been instantiated.", typeof(TEntity)));

            var handler = _instance.EntityChangedEvent;
            if (handler != null)
                handler(_instance, e);
        }

        public static void InvokeEntityChangedOnSyncEvent(EntityChangedEventArgs<TEntity> e)
        {
            if (_instance == null) throw new InvalidOperationException(string.Format("ServiceRepositoryBase<{0}> has never been instantiated.", typeof(TEntity)));

            var handler = _instance.EntityChangedOnSyncEvent;
            if (handler != null)
                handler(_instance, e);
        }

        public static void InvokeEntityDeletedOnSyncEvent(EntityDeletedEventArgs<TEntity> e)
        {
            if (_instance == null) throw new InvalidOperationException(string.Format("ServiceRepositoryBase<{0}> has never been instantiated.", typeof(TEntity)));

            var handler = _instance.EntityDeletedOnSyncEvent;
            if (handler != null)
                handler(_instance, e);
        }

        public static void InvokeEntityDeletedEvent(EntityDeletedEventArgs<TEntity> e)
        {
            if (_instance == null) throw new InvalidOperationException(string.Format("ServiceRepositoryBase<{0}> has never been instantiated.", typeof(TEntity)));

            var handler = _instance.EntityDeletedEvent;
            if (handler != null)
                handler(_instance, e);
        }

        #endregion

        public abstract void Save(Guid sessionToken, TEntity item, bool notifySubscribers);
        public abstract void Delete(Guid sessionToken, Guid id);

        protected abstract ServiceCommandClient CreateCommandClient();
        protected abstract IMessageServiceClient GetSyncEventClient();

        public virtual ISyncCompleteData Sync(Guid sessionToken, DateTime? syncTime)
        {
            lock (ServiceRepositoryHelper.SyncRoot)
            {
                var client = GetSyncEventClient();
                try
                {
                    StartSync(sessionToken, client, syncTime);
                }
                catch
                {
                    client.Abort();
                    //Issue.Register(exception, false);
                    throw;
                }
                return WaitSyncComplete(client);
            }
        }

        private ISyncCompleteData WaitSyncComplete(IMessageServiceClient client)
        {
            var seconds = Helper.Settings.GetSetting("WcfTimeoutSeconds", DefaultWcfTimeoutSeconds);
            if (!SyncCompleteEvent.WaitOne(seconds * 1000))
            {
                client.Close();
                throw new TimeoutException(string.Format("Sync {1} did not complete in {0} seconds.", seconds, typeof(TEntity)));
            }
            client.Close();
            //if (SyncCompleteData == null) throw new InvalidOperationException("SyncInfo has not been set.");
            Debug.Assert(SyncCompleteData != null, "SyncInfo has not been set.");
            return SyncCompleteData;
        }

        public void OnSyncCompleteEvent(object sender, ISyncCompleteData syncCompleteData)
        {
            SyncCompleteData = syncCompleteData;
            SyncCompleteEvent.Set();
        }

        private void StartSync(Guid sessionToken, IMessageServiceClient client, DateTime? serverStoreTime)
        {
            client.Sync<TEntity>(sessionToken, serverStoreTime);
        }

        public string OutgoingCommandQueueName
        {
            get
            {
                var client = CreateCommandClient();
                var endpointAddress = client.Endpoint.Address;
                return string.Format("{0}{1}", endpointAddress.Uri.Host, endpointAddress.Uri.LocalPath.Replace("/private/", @"\private$\"));
            }
        }
    }
}