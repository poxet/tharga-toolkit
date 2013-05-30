using System.Messaging;
using System.Threading.Tasks;

namespace Tharga.Toolkit.LocalStorage.Business
{
    public class QueueHandler
    {
        private readonly string _queueName;

        public QueueHandler(string queueName)
        {
            _queueName = queueName;
        }

        public string QueueName { get { return _queueName; } }

        public async Task<int> GetPendingMessageCount()
        {
            var task = Task.Factory.StartNew(() =>
                {
                    var fullQueueName = string.Format("FormatName:Direct=OS:{0}", QueueName);
                    var mq = new MessageQueue(fullQueueName, QueueAccessMode.ReceiveAndAdmin);
                    return mq.GetAllMessages().Length;
                });
            await task;
            return task.Result;
        }
    }
}