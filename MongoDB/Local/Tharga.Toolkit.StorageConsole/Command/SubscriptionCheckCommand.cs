using System;
using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;

namespace Tharga.Toolkit.StorageConsole.Command
{
    public class SubscriptionCheckCommand : ActionCommandBase
    {
        private readonly SubscriptionHandler _subscriptionHandler;

        internal SubscriptionCheckCommand(IConsole console, SubscriptionHandler subscriptionHandler)
            : base(console, "check", "Check the subscription")
        {
            _subscriptionHandler = subscriptionHandler;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            try
            {
                await _subscriptionHandler.CheckSubscriptionAsync();
            }
            catch (SystemException exception)
            {
                if (string.Compare(exception.Message, "Cannot check subscription if there is no token.", StringComparison.InvariantCultureIgnoreCase) == 0)
                    OutputInformation("There is no active subscription.");
                else
                    OutputInformation(exception.Message);
            }

            return true;
        }
    }
}