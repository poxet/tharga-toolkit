using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Reflection;
using System.ServiceModel;
using System.Threading.Tasks;
using Tharga.Toolkit.LocalStorage.Entity;
using Tharga.Toolkit.LocalStorage.Exceptions;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Business
{
    public enum SyncMode { Always, Session, Never }

    public abstract class BusinessBase<TEntity>
        where TEntity : IEntity
    {
        private bool _isSyncRunning;

        private readonly ILocalRepository<TEntity> _localRepository;
        private readonly IServiceRepository<TEntity> _serviceRepository;
        private readonly ISubscriptionHandler _subscriptionHandler;

        protected ILocalRepository<TEntity> LocalRepository { get { return _localRepository; } }
        protected IServiceRepository<TEntity> ServiceRepository { get { return _serviceRepository; } }
        public ISubscriptionHandler SubscriptionHandler { get { return _subscriptionHandler; } }

        #region Event

        public event EventHandler<EntityChangedEventArgs<TEntity>> EntityChanged;
        public event EventHandler<EntityDeletedEventArgs<TEntity>> EntityDeleted;
        public event EventHandler<SyncStartedEventArgs> SyncStarted;
        public event EventHandler<SyncCompleteEventArgs> SyncCompleted;

        public class SyncStartedEventArgs : EventArgs
        {
            public DateTime? PreviousServerStoreTime { get; private set; }
            public DateTime StartTime { get; private set; }

            public SyncStartedEventArgs(DateTime? previousServerStoreTime)
            {
                PreviousServerStoreTime = previousServerStoreTime;
                StartTime = DateTime.UtcNow;
            }
        }

        public class SyncCompleteEventArgs : EventArgs
        {
            public int EntityChangedCount { get; private set; }
            public int EntityDeletedCount { get; private set; }
            public int EntityResendCount { get; private set; }
            public DateTime StartTime { get; private set; }
            public DateTime EndTime { get; private set; }
            public DateTime? ServerStoreTime { get; private set; }
            public TimeSpan Duration { get { return EndTime - StartTime; } }
            public Exception Exception { get; private set; }

            public SyncCompleteEventArgs(ISyncCompleteData syncCompleteData, int entityResendCount, DateTime startTime, Exception exception)
            {
                EntityChangedCount = syncCompleteData != null ? syncCompleteData.EntityChangedCount : -1;
                EntityDeletedCount = syncCompleteData != null ? syncCompleteData.EntityDeletedCount : -1;
                StartTime = startTime;
                EndTime = DateTime.UtcNow;
                EntityResendCount = entityResendCount;
                if (syncCompleteData != null && syncCompleteData.LastServerStoreTime != null)
                    ServerStoreTime = syncCompleteData.LastServerStoreTime.Value;
                Exception = exception;
            }
        }

        private void InvokeSyncStartedEvent(SyncStartedEventArgs e)
        {
            var handler = SyncStarted;
            if (handler != null)
                handler(this, e);
        }

        private void InvokeSyncCompleted(SyncCompleteEventArgs e)
        {
            var handler = SyncCompleted;
            if (handler != null)
                handler(this, e);
        }

        protected void InvokeEntityChangedEvent(EntityChangedEventArgs<TEntity> e)
        {
            var handler = EntityChanged;
            if (handler != null)
                handler(this, e);
        }

        private void InvokeEntityDeletedEvent(EntityDeletedEventArgs<TEntity> e)
        {
            var handler = EntityDeleted;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        public ISubscriptionHandler Subscription { get { return _subscriptionHandler; } }

        protected BusinessBase(ILocalRepository<TEntity> localRepository, ISubscriptionHandler subscriptionHandler)
            : this(localRepository, null, subscriptionHandler)
        {
            
        }

        protected BusinessBase(ILocalRepository<TEntity> localRepository, IServiceRepository<TEntity> serviceRepository, ISubscriptionHandler subscriptionHandler)
        {
            if (localRepository == null) throw new ArgumentNullException("localRepository");

            _localRepository = localRepository;
            _serviceRepository = serviceRepository;
            _subscriptionHandler = subscriptionHandler;

            if (_serviceRepository != null)
            {
                _serviceRepository.EntityChangedEvent += ServiceRepositoryEntityChangedEvent;
                _serviceRepository.EntityChangedOnSyncEvent += ServiceRepositoryEntityChangedOnSyncEvent;
                _serviceRepository.EntityDeletedEvent += ServiceRepositoryEntityDeletedEvent;
                _serviceRepository.EntityDeletedOnSyncEvent += ServiceRepositoryEntityDeletedOnSyncEvent;
            }

            //EnableSync = true;
        }

        private void ServiceRepositoryEntityDeletedOnSyncEvent(object sender, EntityDeletedEventArgs<TEntity> e)
        {
            try
            {
                if (e.SaveLocation == null) throw new SystemException("SaveLocation has not been set. The origin of this event cannot be determined.");

                DeleteLocally(e.RealmId, e.Entity.Id);
            }
            catch (Exception exception)
            {
                e.Exception = exception;
            }
            InvokeEntityDeletedEvent(e);
        }

        private void ServiceRepositoryEntityDeletedEvent(object sender, EntityDeletedEventArgs<TEntity> e)
        {
            try
            {
                if (e.SaveLocation == null) throw new SystemException("SaveLocation has not been set. The origin of this event cannot be determined.");

                lock (BusinessBaseHelper.SyncRoot)
                {
                    DeleteLocally(e.RealmId, e.Entity.Id);

                    var syncTime = GetSyncTime(e.RealmId);
                    if (syncTime == e.PreviousServerStoreTime && e.Entity.StoreInfo.ServerStoreTime != null)
                    {
                        SetSyncTime(e.RealmId, e.Entity.StoreInfo.ServerStoreTime.Value);
                    }
                    else if (syncTime == null || syncTime < e.PreviousServerStoreTime)
                    {
                        //Out of sync
                        Sync(SyncMode.Always);
                    }
                }

            }
            catch (Exception exception)
            {
                e.Exception = exception;
            }
            InvokeEntityDeletedEvent(e);
        }

        private void ServiceRepositoryEntityChangedOnSyncEvent(object sender, EntityChangedEventArgs<TEntity> e)
        {
            try
            {
                if (e.SaveLocation == null) throw new ArgumentException("SaveLocation has not been set. The origin of this event cannot be determined.");
                if (e.SaveLocation.Value != Location.Server) throw new ArgumentException("SaveLocation for sync event should always be server.");

                ValidationCheck(e.Entity);

                SaveLocally(e.RealmId, e.Entity);
            }
            catch (Exception exception)
            {
                e.Exception = exception;
            }
            InvokeEntityChangedEvent(e);
        }

        private void ServiceRepositoryEntityChangedEvent(object sender, EntityChangedEventArgs<TEntity> e)
        {
            try
            {
                if (e.SaveLocation == null) throw new ArgumentException("SaveLocation has not been set. The origin of this event cannot be determined.");

                ValidationCheck(e.Entity);

                lock (BusinessBaseHelper.SyncRoot)
                {
                    SaveLocally(e.RealmId, e.Entity);

                    var syncTime = GetSyncTime(e.RealmId);
                    if (syncTime == e.PreviousServerStoreTime && e.Entity.StoreInfo.ServerStoreTime != null)
                    {
                        SetSyncTime(e.RealmId, e.Entity.StoreInfo.ServerStoreTime.Value);
                    }
                    else if (syncTime == null || syncTime < e.PreviousServerStoreTime)
                    {
                        //Out of sync
                        Sync(SyncMode.Always);
                    }
                }
            }
            catch (Exception exception)
            {
                e.Exception = exception;
            }
            InvokeEntityChangedEvent(e);
        }

        protected virtual IEnumerable<TEntity> GetAll(SyncMode syncMode = SyncMode.Session)
        {
            return GetAllEx(Guid.Empty, syncMode);
        }

        protected IEnumerable<TEntity> GetAllEx(Guid realmId, SyncMode syncMode = SyncMode.Session)
        {
            Sync(syncMode);
            return _localRepository.GetAll(realmId);
        }

        protected virtual IEnumerable<TEntity> GetOutOfSync()
        {
            return GetOutOfSyncEx(Guid.Empty);
        }

        protected IEnumerable<TEntity> GetOutOfSyncEx(Guid realmId)
        {
            return _localRepository.GetOutOfSync(realmId);
        }

        protected virtual void Save(TEntity entity, bool notifySubscribers)
        {
            SaveEx(Guid.Empty, Guid.Empty, entity, notifySubscribers);
        }

        protected void SaveEx(Guid realmId, Guid sessionToken, TEntity entity, bool notifySubscribers)
        {
            ValidationCheck(entity);
            //TOOD: Check business rules

            AssignId(entity);
            AssignStoreInfo(entity);

            SaveLocally(realmId, entity);
            SaveOnServer(sessionToken, entity, notifySubscribers);

            InvokeEntityChangedEvent(EntityChangedEventArgs<TEntity>.CreateLocal(realmId, entity));
        }

        protected virtual void ValidationCheck(TEntity entity)
        {
            var entityWithValidation = entity as IEntityWithValidation;
            if (entityWithValidation == null) return;
            if (!entityWithValidation.IsValid)
                throw new EntityInvalidException(entityWithValidation.Error);
        }

        protected void AssignId(TEntity entity)
        {
            if (entity.Id == Guid.Empty)
            {
                var property = entity.GetType().GetProperty("Id");

                if (property == null)
                    property = entity.GetType().GetProperty("Id", BindingFlags.Instance);

                if (property == null)
                    property = entity.GetType().GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic);

                if (property != null)
                    property.SetValue(entity, Guid.NewGuid());
            }

            if (entity.Id == Guid.Empty) throw new ArgumentException(string.Format("Id has not been set for entity. It has to be set by the entity object {0} since it could not be set by the save function.", entity.GetType().Name));
        }

        protected static void AssignStoreInfo(TEntity entity)
        {
            if (entity.StoreInfo == null)
            {
                var property = entity.GetType().GetProperty("StoreInfo");

                if (property == null)
                    property = entity.GetType().GetProperty("StoreInfo", BindingFlags.Instance);

                if (property == null)
                    property = entity.GetType().GetProperty("StoreInfo", BindingFlags.Instance | BindingFlags.NonPublic);

                if (property != null)
                    property.SetValue(entity, new StoreInfo());
            }

            if (entity.StoreInfo == null) throw new ArgumentNullException("entity", string.Format("StoreInfo does not exist for entity. It has to be set by the entity object {0} since it could not be set by the save function.", entity.GetType().Name));

            entity.StoreInfo.SetLocalSaveInfo();
        }

        protected void SaveLocally(Guid realmId, TEntity entity)
        {
            _localRepository.Save(realmId, entity);
        }

        protected void SaveOnServer(Guid sessionToken, TEntity entity, bool notifySubscribers)
        {
            if (_serviceRepository != null)
                _serviceRepository.Save(sessionToken, entity, notifySubscribers);
        }

        private void DeleteLocally(Guid realmId, Guid id)
        {
            _localRepository.Delete(realmId, id);
        }

        private bool ShallSync(SyncMode syncMode)
        {
            switch (syncMode)
            {
                case SyncMode.Always:
                    return true;
                case SyncMode.Session:
                    return !BusinessBaseHelper.IsSynced<TEntity>();
                default:
                    throw new ArgumentOutOfRangeException(string.Format("Unknown sync mode {0}.", syncMode));
            }
        }

        private DateTime? GetSyncTime(Guid realmId)
        {
            var sh = new SyncHandler();
            return sh.GetSyncTime(realmId, typeof(TEntity));
        }

        private void SetSyncTime(Guid realmId, DateTime syncTime)
        {
            var sh = new SyncHandler();
            sh.SetSyncTime(realmId, typeof(TEntity), syncTime);
        }

        protected virtual void Sync(SyncMode syncMode)
        {
            SyncEx(Guid.Empty, Guid.Empty, syncMode);
        }

        protected void SyncEx(Guid realmId, Guid sessionToken, SyncMode syncMode)
        {
            if (_serviceRepository == null)
                return;

            if (!_subscriptionHandler.IsOnline)
                return;

            if (!ShallSync(syncMode))
                return;

            lock (BusinessBaseHelper.SyncRoot)
            {
                if (_isSyncRunning)
                    return;
                _isSyncRunning = true;

                Exception exception = null;
                ISyncCompleteData syncData = null;
                var resendCounter = 0;

                var syncTime = GetSyncTime(realmId);
                var syncStartedEventArgs = new SyncStartedEventArgs(syncTime);
                InvokeSyncStartedEvent(syncStartedEventArgs);
                try
                {
                    //TODO: Remove this delay simulator in production
                    //System.Threading.Thread.Sleep(5000);

                    syncData = _serviceRepository.Sync(sessionToken, syncTime);

                    resendCounter = ResendEntities();

                    if (syncData.LastServerStoreTime != null)
                        SetSyncTime(realmId, syncData.LastServerStoreTime.Value);
                    BusinessBaseHelper.SetSynced<TEntity>();
                }
                catch (EndpointNotFoundException exp)
                {
                    _subscriptionHandler.GoOffline();
                    exception = exp;
                }
                catch (TimeoutException exp)
                {
                    exception = exp;
                }
                catch (UnableToResendException exp)
                {
                    exception = exp;
                }
                finally
                {
                    _isSyncRunning = false;
                }

                InvokeSyncCompleted(new SyncCompleteEventArgs(syncData, resendCounter, syncStartedEventArgs.StartTime, exception));
            }
        }

        private int ResendEntities()
        {
            if (_serviceRepository == null)
                return -1;

            var entitiesOutOfSync = GetOutOfSync().OrderBy(x => x.StoreInfo.LocalStoreTime).ToList();
            if (!entitiesOutOfSync.Any()) return 0;

            var allButLast = entitiesOutOfSync.TakeAllButLast();
            var lastEntity = entitiesOutOfSync.Last();

            CheckOutgoingQueue();

            var sessionToken = Guid.Empty; //TODO: What session should we use when resending an item?

            var resendCounter = 0;
            foreach (var entity in allButLast)
            {
                _serviceRepository.Save(sessionToken, entity, false);
                resendCounter++;
            }

            if (lastEntity != null)
            {
                _serviceRepository.Save(sessionToken, lastEntity, true);
                resendCounter++;
            }

            return resendCounter;
        }

        private void CheckOutgoingQueue()
        {
            var count = GetOutgoingQueueMessageCount();

            int maxCount;
            if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["MaxPendingResendItems"], out maxCount))
                maxCount = 10;

            if (count >= maxCount)
                throw new UnableToResendException(count, maxCount);
        }

        private int GetOutgoingQueueMessageCount()
        {
            if (_serviceRepository == null)
                return -1;

            var queueName = string.Format(@"FormatName:Direct=OS:{0}", _serviceRepository.OutgoingCommandQueueName);
            var mq = new MessageQueue(queueName, QueueAccessMode.ReceiveAndAdmin);
            return mq.GetAllMessages().Length;
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = Task.Factory.StartNew(() => Delete(id));
            await task;
        }

        protected virtual void Delete(Guid id)
        {
            DeleteEx(Guid.Empty, Guid.Empty, id);
        }

        protected void DeleteEx(Guid realmId, Guid sessionToken, Guid id)
        {
            if (_serviceRepository != null)
            {
                //Send an event to delete on server, when the callback arrives the item is also deleted locally.
                //TODO: Perhaps mark the item as deleted locally, or delete it directly. (Remember to fire event if so) Consider the different methids and the impact. What if the system works offline for a longer time, what happens with deleted items then?
                _serviceRepository.Delete(sessionToken, id);
            }
            else
            {
                var entity = GetAll().Single(x => x.Id == id); //TODO: Create a Get(Id) function (_localRepositoryInstance.Get(id)) instead of getting all and single one out.
                DeleteLocally(realmId, id);
                InvokeEntityDeletedEvent(EntityDeletedEventArgs<TEntity>.CreateLocal(realmId, entity));
            }
        }

        public async Task ResetAsync()
        {
            var task = Task.Factory.StartNew(Reset);
            await task;
        }

        protected virtual void Reset()
        {
            ResetEx(Guid.Empty);
        }

        protected void ResetEx(Guid realmId)
        {
            _localRepository.DeleteAll(realmId);
            BusinessBaseHelper.ClearSynced<TEntity>();
            var sh = new SyncHandler();
            sh.ClearSyncTime(realmId, typeof(TEntity));
        }

        public async Task SyncAsync(SyncMode syncMode)
        {
            var task = new Task(() => Sync(syncMode));
            task.Start();
            await task;
        }

        public async Task SaveAsync(TEntity entity)
        {
            var task = Task.Factory.StartNew(() => Save(entity, true));
            await task;
        }

        public async Task SaveBatchAsync(IEnumerable<TEntity> entities)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var first = true;
                var prev = default(TEntity);
                var enumerator = entities.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (!first)
                        Save(prev, false);
                    first = false;
                    prev = enumerator.Current;
                }
                Save(prev, true);
            });
            await task;
        }

        public async Task<List<TEntity>> GetAllAsync(SyncMode syncMode = SyncMode.Session)
        {
            var task = Task<List<TEntity>>.Factory.StartNew(() => GetAll(syncMode).ToList());
            return await task;
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            var task = Task<TEntity>.Factory.StartNew(() =>
                {
                    var entity = _localRepository.Get(Guid.Empty, id);
                    if (entity == null)
                        entity = GetAll().FirstOrDefault(x => x.Id == id);
                    return entity;
                });

            return await task;
        }
    }
}
