using System.Threading.Tasks;
using SampleWpfClient.Model;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient.Command
{
    public abstract class CommandBase<TEntity> : CommandBase
        where TEntity : IEntity
    {
        private bool _synchronizing;
        protected bool Synchronizing
        {
            get { return _synchronizing; }
            private set
            {
                if (_synchronizing != value)
                {
                    _synchronizing = value;
                    RefreshCanExecute();
                }
            }
        }

        protected CommandBase(BusinessBase<TEntity> business)
            : base(business.Subscription)
        {
            business.SyncStarted += business_SyncStarted;
            business.SyncCompleted += business_SyncCompleted;
        }

        void business_SyncStarted(object sender, BusinessBase<TEntity>.SyncStartedEventArgs e)
        {
            Synchronizing = true;
        }

        void business_SyncCompleted(object sender, BusinessBase<TEntity>.SyncCompleteEventArgs e)
        {
            Synchronizing = false;            
        }
    }

    public class RefreshCommand<TEntity> : CommandBase<TEntity>
        where TEntity : class, IEntity
    {
        private readonly BusinessBase<TEntity> _business;
        private readonly ModelBase<TEntity> _model;

        public RefreshCommand(BusinessBase<TEntity> business, ModelBase<TEntity> model)
            :base(business)
        {
            _business = business;
            _model = model;
        }

        public override bool CanExecute(object parameter)
        {
            if (_business is RealmBusinessBase<TEntity> && _business.SubscriptionHandler.Session == null)
                return false;

            return _business.Subscription.IsOnline && !Executing && !Synchronizing;
        }

        protected override async Task DoExecute(object parameter)
        {
            await _business.ResetAsync();
            var list = await _business.GetAllAsync();
            _model.Items.Set(list);
        }
    }
}