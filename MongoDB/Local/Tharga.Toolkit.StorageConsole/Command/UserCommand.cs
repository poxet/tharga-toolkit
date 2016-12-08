using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;

namespace Tharga.Toolkit.StorageConsole.Command
{
    public class UserCommand : ContainerCommandBase
    {
        internal UserCommand(IConsole console, SubscriptionHandler subscriptionHandler)
            : base("user")
        {
            RegisterCommand(new UserLogonCommand(console, subscriptionHandler));
            RegisterCommand(new UserLogoffCommand(console, subscriptionHandler));
        }
    }
}