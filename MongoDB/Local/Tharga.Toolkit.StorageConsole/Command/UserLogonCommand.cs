using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.StorageConsole.Command
{
    public class UserLogonCommand : ActionCommandBase
    {
        private readonly ISubscriptionHandler _subscriptionHandler;

        public UserLogonCommand(IConsole console, ISubscriptionHandler subscriptionHandler)
            : base("logon", "Creates a session for the user")
        {
            _subscriptionHandler = subscriptionHandler;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var userName = QueryParam<string>("UserName", GetParam(paramList, index++));
            var password = QueryParam<string>("Password", GetParam(paramList, index++));

            await _subscriptionHandler.CreateSession(userName, password);
            return true;
        }

        public override bool CanExecute(out string reasonMessage)
        {
            reasonMessage = "You need to logoff before calling this command.";
            return _subscriptionHandler.Session == null;
        }
    }
}