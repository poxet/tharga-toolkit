using Tharga.Toolkit.Console.Command.Base;

namespace Tharga.Toolkit.StorageConsole.Command
{
    public class QueueCommand : ContainerCommandBase
    {
        internal QueueCommand(IConsole console, string queueName)
            : base("queue")
        {
            RegisterCommand(new QueueInfoCommand(console, queueName));
        }
    }
}