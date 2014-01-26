using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.StorageConsole.Command
{
    public class UserLogonCommand : ActionCommandBase
    {
        private readonly ISubscriptionHandler _subscriptionHandler;

        public UserLogonCommand(IConsole console, ISubscriptionHandler subscriptionHandler)
            : base(console, "logon", "Creates a session for the user")
        {
            _subscriptionHandler = subscriptionHandler;
        }

        public async override Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var userName = QueryParam<string>("UserName", GetParam(paramList, index++));
            var password = QueryParam<string>("Password", GetParam(paramList, index++));

            await _subscriptionHandler.CreateSession(userName, password);
            return true;
        }

        protected override string GetCanExecuteFaileMessage()
        {
            return string.Format("You need to logoff before calling this command.");
        }

        public override bool CanExecute()
        {
            return _subscriptionHandler.Session == null;
        }
    }
}