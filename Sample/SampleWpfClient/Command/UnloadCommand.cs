using System.Threading.Tasks;
using SampleWpfClient.Model;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient.Command
{
    public class UnloadCommand<TEntity> : CommandBase<TEntity>
        where TEntity : class, IEntity
    {
        private readonly BusinessBase<TEntity> _business;
        private readonly ModelBase<TEntity> _model;

        public UnloadCommand(BusinessBase<TEntity> business, ModelBase<TEntity> model)
            : base(business)
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
            _model.Items.Clear();
        }
    }
}