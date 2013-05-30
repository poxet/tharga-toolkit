using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.StorageConsole.Command
{
    public class UserLogoffCommand : ActionCommandBase
    {
        private readonly ISubscriptionHandler _subscriptionHandler;

        public UserLogoffCommand(IConsole console, ISubscriptionHandler subscriptionHandler)
            : base(console, "logoff", "Ends the user session")
        {
            _subscriptionHandler = subscriptionHandler;
        }

        public async override Task<bool> InvokeAsync(string paramList)
        {
            await _subscriptionHandler.EndSession();
            return true;
        }

        protected override string GetCanExecuteFaileMessage()
        {
            return string.Format("You need to logon before calling this command.");
        }

        public override bool CanExecute()
        {
            return _subscriptionHandler.Session != null;
        }
    }
}