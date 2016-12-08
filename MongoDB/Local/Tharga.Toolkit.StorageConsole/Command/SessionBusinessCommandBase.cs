using System.Linq;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.StorageConsole.Command
{
    public abstract class SessionBusinessCommandBase<THandler, TEntity> : BusinessCommandBase<THandler, TEntity>
        where THandler : BusinessBase<TEntity>
        where TEntity : IEntity
    {
        private readonly THandler _business;

        protected SessionBusinessCommandBase(string name, THandler business)
            : base(name, business)
        {
            _business = business;
            ((ActionCommandBase) GetCommand("List")).SetCanExecute(IsLoggedOn);
            ((ActionCommandBase) GetCommand("Sync")).SetCanExecute(IsLoggedOn);
            ((ActionCommandBase) GetCommand("Reset")).SetCanExecute(IsLoggedOn);
            ((ActionCommandBase) GetCommand("Delete")).SetCanExecute(IsLoggedOn);
        }

        private bool IsLoggedOn()
        {
            return _business.SubscriptionHandler.Session != null;
        }

        public override bool CanExecute()
        {
            string responseMessage;
            return SubCommands.Any(x => x.CanExecute(out responseMessage));
        }
    }
}