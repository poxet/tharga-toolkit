using System;
using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.StorageConsole.Command
{
    class ExitCommand : ActionCommandBase
    {
        private Action _stopAction;
        private readonly ISubscriptionHandler _subscriptionHandler;

        internal ExitCommand(IConsole console, Action stopAction, ISubscriptionHandler subscriptionHandler)
            : base(console, "exit", "Exit from the console.")
        {
            _stopAction = stopAction;
            _subscriptionHandler = subscriptionHandler;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            await _subscriptionHandler.StopSubscriptionAsync ();
            _stopAction();
            return true;
        }

        protected internal void SetStopAction(Action stopAction)
        {
            _stopAction = stopAction;
        }
    }
}