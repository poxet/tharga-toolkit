using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;

namespace Tharga.Toolkit.StorageConsole.Command
{
    public class SubscriptionStartCommand : ActionCommandBase
    {
        private readonly SubscriptionHandler _subscriptionHandler;

        internal SubscriptionStartCommand(IConsole console, SubscriptionHandler subscriptionHandler)
            : base("start", "Starts the subscription")
        {
            _subscriptionHandler = subscriptionHandler;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            await _subscriptionHandler.StartSubscriptionAsync();
            return true;
        }
    }
}