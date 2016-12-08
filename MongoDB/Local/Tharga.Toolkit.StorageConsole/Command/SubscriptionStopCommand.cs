using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;

namespace Tharga.Toolkit.StorageConsole.Command
{
    public class SubscriptionStopCommand : ActionCommandBase
    {
        private readonly SubscriptionHandler _subscriptionHandler;

        internal SubscriptionStopCommand(IConsole console, SubscriptionHandler subscriptionHandler)
            : base("stop", "Stops the subscription")
        {
            _subscriptionHandler = subscriptionHandler;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            await _subscriptionHandler.StopSubscriptionAsync();
            return true;
        }
    }
}