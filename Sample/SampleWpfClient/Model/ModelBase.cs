using System.Linq;
using System.Threading.Tasks;
using Tharga.Toolkit;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient.Model
{
    public abstract class ModelBase<TEntity>
        where TEntity : class, IEntity
    {
        protected readonly BusinessBase<TEntity> _business;
        public SafeObservableCollection<TEntity> Items { get; private set; }

        protected ModelBase(BusinessBase<TEntity> business, bool loadOnStartup)
        {
            Items = new SafeObservableCollection<TEntity>();
            _business = business;
            _business.EntityChanged += business_EntityChanged;
            _business.EntityDeleted += _business_EntityDeleted;

            _business.Subscription.SubscriptionStartedEvent += Subscription_SubscriptionStartedEvent;
            //_business.Subscription.SubscriptionStoppedEvent += Subscription_SubscriptionStoppedEvent;

            if (loadOnStartup)
                LoadAsync();
        }

        //void Subscription_SubscriptionStoppedEvent(object sender, Tharga.Toolkit.LocalStorage.Entity.SubscriptionStoppedEventArgs e)
        //{
        //    Items.Clear();
        //}

        protected async virtual void Subscription_SubscriptionStartedEvent(object sender, Tharga.Toolkit.LocalStorage.Entity.SubscriptionStartedEventArgs e)
        {
            await _business.SyncAsync(SyncMode.Session);
        }

        protected async Task LoadAsync()
        {
            var list = await _business.GetAllAsync();
            Items.Set(list);
        }

        void business_EntityChanged(object sender, Tharga.Toolkit.LocalStorage.Entity.EntityChangedEventArgs<TEntity> e)
        {
            var item = Items.FirstOrDefault(x => x.Id == e.Entity.Id);
            if (item == null)
                Items.Add(e.Entity);
            else
                Items.Replace(item, e.Entity);
        }

        void _business_EntityDeleted(object sender, Tharga.Toolkit.LocalStorage.Entity.EntityDeletedEventArgs<TEntity> e)
        {
            var item = Items.FirstOrDefault(x => x.Id == e.Entity.Id);
            if (item == null) return;
            Items.Remove(item);
        }
    }
}