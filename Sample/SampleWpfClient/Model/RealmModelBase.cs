using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Entity;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient.Model
{
    public abstract class RealmModelBase<TEntity> : ModelBase<TEntity>
        where TEntity : class, IEntity
    {
        private readonly bool _loadOnSessionCreated;

        protected RealmModelBase(RealmBusinessBase<TEntity> business, bool loadOnSessionCreated) 
            : base(business, false)
        {
            _loadOnSessionCreated = loadOnSessionCreated;
            _business.Subscription.SessionCreatedEvent += Subscription_SessionCreatedEvent;
            _business.Subscription.SessionEndedEvent += Subscription_SessionEndedEvent;
        }

        void Subscription_SessionEndedEvent(object sender, SessionEndedEventArgs e)
        {
            Items.Clear();
        }

        private async void Subscription_SessionCreatedEvent(object sender, SessionCreatedEventArgs e)
        {
            if ( _loadOnSessionCreated)
                await LoadAsync();
            await _business.SyncAsync(SyncMode.Session);
        }

        protected async override void Subscription_SubscriptionStartedEvent(object sender, Tharga.Toolkit.LocalStorage.Entity.SubscriptionStartedEventArgs e)
        {
            //if (_business.Subscription.Session != null)
            //    await _business.SyncAsync(SyncMode.Session);
        }
    }
}