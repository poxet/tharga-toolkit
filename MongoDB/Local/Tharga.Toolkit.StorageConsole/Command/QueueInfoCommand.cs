using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;

namespace Tharga.Toolkit.StorageConsole.Command
{
    class QueueInfoCommand : ActionCommandBase
    {
        private readonly string _queueName;

        internal QueueInfoCommand(IConsole console, string queueName)
            : base("info","Shows information about the queue.")
        {
            _queueName = queueName;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var qh = new QueueHandler(_queueName);

            OutputInformation("Queue name: {0}", qh.QueueName);
            OutputInformation("Pending messages: {0}", await qh.GetPendingMessageCount());

            return true;
        }
    }
}