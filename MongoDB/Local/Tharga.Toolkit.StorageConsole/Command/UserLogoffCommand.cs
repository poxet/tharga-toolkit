using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.StorageConsole.Command
{
    public class UserLogoffCommand : ActionCommandBase
    {
        private readonly ISubscriptionHandler _subscriptionHandler;

        public UserLogoffCommand(IConsole console, ISubscriptionHandler subscriptionHandler)
            : base("logoff", "Ends the user session")
        {
            _subscriptionHandler = subscriptionHandler;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            await _subscriptionHandler.EndSession();
            return true;
        }

        public override bool CanExecute(out string reasonMessage)
        {
            reasonMessage = "You need to logon before calling this command.";
            return _subscriptionHandler.Session != null;
        }
    }
}