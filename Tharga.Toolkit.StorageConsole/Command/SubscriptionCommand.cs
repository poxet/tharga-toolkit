using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;

namespace Tharga.Toolkit.StorageConsole.Command
{
    public class SubscriptionCommand : ContainerCommandBase
    {
        internal SubscriptionCommand(IConsole console, SubscriptionHandler subscriptionHandler)
            : base(console, "subscription")
        {
            RegisterCommand(new SubscriptionStartCommand(console, subscriptionHandler));
            RegisterCommand(new SubscriptionStopCommand(console, subscriptionHandler));
            RegisterCommand(new SubscriptionCheckCommand(console, subscriptionHandler));
        }
    }
}