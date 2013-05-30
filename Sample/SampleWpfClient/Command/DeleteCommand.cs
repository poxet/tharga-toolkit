using System;
using System.Threading.Tasks;
using SampleWpfClient.Model;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient.Command
{
    public class DeleteCommand<TEntity> : CommandBase
        where TEntity : class, IEntity
    {
        private readonly BusinessBase<TEntity> _business;
        private readonly ModelBase<TEntity> _model;

        public DeleteCommand(BusinessBase<TEntity> business, ModelBase<TEntity> model)
            :base(business.Subscription)
        {
            _business = business;
            _model = model;
        }

        public override bool CanExecute(object parameter)
        {
            return _business.Subscription.IsOnline && !Executing;
        }

        protected override async Task DoExecute(object parameter)    
        {
            throw new NotImplementedException();
            //await _business.ResetAsync();
            //var list = await _business.GetAllAsync();
            //_model.Items.Set(list);
        }
    }
}