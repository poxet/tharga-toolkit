using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.Console.Command.Base
{
    public abstract class SessionActionCommandBase<TEntity> : ActionCommandBase 
        where TEntity : IEntity
    {
        private readonly RealmBusinessBase<TEntity> _business;

        protected SessionActionCommandBase(RealmBusinessBase<TEntity> business, string name, string description) 
            : base(name, description)
        {
            _business = business;
        }

        protected SessionActionCommandBase(RealmBusinessBase<TEntity> business, IConsole console, string name, string description)
            : base(console, name, description)
        {
            _business = business;
        }

        protected SessionActionCommandBase(RealmBusinessBase<TEntity> business, IConsole console, string name, string description, string helpText) 
            : base(console, name, description, helpText)
        {
            _business = business;
        }

        protected override string GetCanExecuteFaileMessage()
        {
            return string.Format("You need to logon before calling this command.");
        }

        public override bool CanExecute()
        {
            return _business.SubscriptionHandler.Session != null;
        }
    }
}