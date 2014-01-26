using System;
using System.Collections.Generic;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Business
{
    public abstract class RealmBusinessBase<TEntity> : BusinessBase<TEntity>
        where TEntity : IEntity
    {
        protected RealmBusinessBase(ILocalRepository<TEntity> localRepository, ISubscriptionHandler subscriptionHandler) 
            : base(localRepository, subscriptionHandler)
        {
            
        }

        protected RealmBusinessBase(ILocalRepository<TEntity> localRepository, IServiceRepository<TEntity> serviceRepository, ISubscriptionHandler subscriptionHandler) 
            : base(localRepository, serviceRepository, subscriptionHandler)
        {
            
        }

        protected override void Reset()
        {
            var session = SubscriptionHandler.Session;

            if (session == null)
                throw new InvalidOperationException("There is no session. Cannot Reset!");

            ResetEx(session.RealmId);
        }

        protected override void Delete(Guid id)
        {
            var session = SubscriptionHandler.Session;

            if (session == null)
                throw new InvalidOperationException("There is no session. Cannot Reset!");

            DeleteEx(session.RealmId, session.SessionToken, id);
        }

        protected override IEnumerable<TEntity> GetOutOfSync()
        {
            var session = SubscriptionHandler.Session;

            if (session == null)
                throw new InvalidOperationException("There is no session. Cannot GetOutOfSync!");

            return GetOutOfSyncEx(session.RealmId);
        }

        protected override void Sync(SyncMode syncMode)
        {
            var session = SubscriptionHandler.Session;

            if (session == null)
                throw new InvalidOperationException("There is no session. Cannot Sync!");

            SyncEx(session.RealmId, session.SessionToken, syncMode);
        }

        protected override void Save(TEntity entity, bool notifySubscribers)
        {
            var session = SubscriptionHandler.Session;

            if (session == null)
                throw new InvalidOperationException("There is no session. Cannot Save!");

            //TODO: Check access

            SaveEx(session.RealmId, session.SessionToken, entity, notifySubscribers);
        }

        protected override IEnumerable<TEntity> GetAll(SyncMode syncMode = SyncMode.Session)
        {
            var session = SubscriptionHandler.Session;

            if (session == null)
                throw new InvalidOperationException("There is no session. Cannot GetAll!");

            return GetAllEx(session.RealmId);
        }
    }
}