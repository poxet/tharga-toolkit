using System;
using System.Messaging;
using System.ServiceModel;
using System.Text;
using MongoDB.Driver;
using Tharga.Toolkit.ServerStorage.CommandBase;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.ServerStorage
{
    public enum OutputLevel //TODO: Why two of the same?
    {
        Default = 0,
        Information = 1,
        Warning = 2,
        Error = 3,
    }

    public class NotificationEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public OutputLevel OutputLevel { get; private set; }

        public NotificationEventArgs(string message, OutputLevel outputLevel = OutputLevel.Default)
        {
            Message = message;
            OutputLevel = outputLevel;
        }
    }

    public class ServiceHandler<T>
    {
        private readonly ServiceHost _serviceHost;
        private readonly ServiceHost _subscriptionServiceHost;

        public ServiceHandler(Type serviceMessageType, Type serviceCommandType)
        {
            ServiceMessageBase<T>.ClientClosingEvent += ServiceEvent_ClientClosingEvent;
            ServiceMessageBase<T>.ClientClosedEvent += ServiceEvent_ClientClosedEvent;
            ServiceMessageBase<T>.SubscriptionStartedEvent += ServiceEvent_SubscriptionStartedEvent;
            ServiceMessageBase<T>.SubscriptionStoppedEvent += ServiceEvent_SubscriptionStoppedEvent;
            ServiceMessageBase<T>.SubscriptionCheckedEvent += ServiceHandler_SubscriptionCheckedEvent;

            ServiceMessageBase<T>.RequestEvent += NevadaServiceEvent_RequestEvent;
            ServiceCommandBase.CommandExecutedEvent += NevadaServiceCommand_CommandExecutedEvent;

            _subscriptionServiceHost = new ServiceHost(serviceMessageType);
            _subscriptionServiceHost.Opened += _subscriptionServiceHost_Opened;
            _subscriptionServiceHost.Closed += _subscriptionServiceHost_Closed;
            _subscriptionServiceHost.Faulted += _subscriptionServiceHost_Faulted;
            _subscriptionServiceHost.UnknownMessageReceived += _subscriptionServiceHost_UnknownMessageReceived;

            _serviceHost = new ServiceHost(serviceCommandType);
            _serviceHost.Opened += _serviceHost_Opened;
            _serviceHost.Closed += _serviceHost_Closed;
            _serviceHost.Faulted += _serviceHost_Faulted;
            _serviceHost.Description.Behaviors.Add(new ErrorServiceBehavior<T>());
            _serviceHost.UnknownMessageReceived += _serviceHost_UnknownMessageReceived;            
        }

        ~ServiceHandler()
        {
            System.Diagnostics.Debug.WriteLine("Destroying ServiceHandler.");
        }

        #region Event


        public static event EventHandler<NotificationEventArgs> NotificationEvent;

        internal static void InvokeNotificationEvent(NotificationEventArgs e)
        {
            try
            {
                var handler = NotificationEvent;
                if (handler != null)
                    handler(null, e);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Swallowed Exception {0}", exception.Message);
            }
        }

        #endregion

        public void Open()
        {
            CreateQueue(GetEndpointAddress());

            _subscriptionServiceHost.Open();
            _serviceHost.Open();

            CheckIfDatabaseIsOnline();
        }

        private string GetEndpointAddress()
        {
            var endpoints = _serviceHost.Description.Endpoints;
            if (endpoints.Count == 0) throw new InvalidOperationException("There are no endpoints defined for the host.");
            return endpoints[0].Address.ToString();
        }

        public void Close()
        {
            if (_serviceHost != null && _serviceHost.State == CommunicationState.Opened)
                _serviceHost.Close();

            if (_subscriptionServiceHost != null && _subscriptionServiceHost.State == CommunicationState.Opened)
                _subscriptionServiceHost.Close();
        }

        private static void CreateQueue(string address)
        {
            var queueName = GetQueueNameFromAddress(address);
            if (MessageQueue.Exists(queueName))
            {
                InvokeNotificationEvent(new NotificationEventArgs(String.Format("Queue {0} already exists.", queueName), OutputLevel.Information));

                //Assign access rights to existing queue
                var queues = MessageQueue.GetPrivateQueuesByMachine(Environment.MachineName);
                foreach (var q in queues)
                    if (queueName.EndsWith(q.QueueName, StringComparison.InvariantCultureIgnoreCase))
                        AssignQueueRights(q);

                return;
            }

            var queue = MessageQueue.Create(queueName, true);
            InvokeNotificationEvent(new NotificationEventArgs(String.Format("Created queue {0}.", queueName), OutputLevel.Information));
            AssignQueueRights(queue);
        }

        private static string GetQueueNameFromAddress(string address)
        {
            var arr = address.Split('/');
            var queueName = String.Format(@".\{0}$\{1}", arr[arr.Length - 2], arr[arr.Length - 1]);
            return queueName;
        }

        private static void AssignQueueRights(MessageQueue queue)
        {
            try
            {
                queue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl, AccessControlEntryType.Allow);
                queue.SetPermissions("ANONYMOUS LOGON", MessageQueueAccessRights.FullControl, AccessControlEntryType.Allow);
            }
            catch (Exception exception)
            {
                InvokeNotificationEvent(new NotificationEventArgs(exception.Message, OutputLevel.Error));
            }
        }

        private static void CheckIfDatabaseIsOnline()
        {
            try
            {
                var client = new MongoClient();
                var server = client.GetServer();
                InvokeNotificationEvent(new NotificationEventArgs(string.Format("Database is online. (State: {0})", server.State), OutputLevel.Information));
            }
            catch (DatabaseOfflineException exception)
            {
                InvokeNotificationEvent(new NotificationEventArgs(exception.Message, OutputLevel.Warning));
            }
        }

        #region Event Handlers


        void _subscriptionServiceHost_Opened(object sender, EventArgs e)
        {
            InvokeNotificationEvent(new NotificationEventArgs("Subscription Service Host has started."));
        }

        void _subscriptionServiceHost_Closed(object sender, EventArgs e)
        {
            InvokeNotificationEvent(new NotificationEventArgs("Subscription Service Host has closed."));
        }

        void _subscriptionServiceHost_Faulted(object sender, EventArgs e)
        {
            InvokeNotificationEvent(new NotificationEventArgs(string.Format("Subscription Service Host Faulted."), OutputLevel.Error));
        }

        void _subscriptionServiceHost_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            InvokeNotificationEvent(new NotificationEventArgs(string.Format("Unknown Message Received to Subscription Service Host. Message: {0}", e.Message), OutputLevel.Warning));
        }

        void _serviceHost_Opened(object sender, EventArgs e)
        {
            InvokeNotificationEvent(new NotificationEventArgs("Command Queue Service Host has started."));
        }

        void _serviceHost_Closed(object sender, EventArgs e)
        {
            InvokeNotificationEvent(new NotificationEventArgs("Command Queue Service Host has closed."));
        }

        void _serviceHost_Faulted(object sender, EventArgs e)
        {
            InvokeNotificationEvent(new NotificationEventArgs(string.Format("Command Queue Service Host Faulted."), OutputLevel.Error));
        }

        void _serviceHost_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            InvokeNotificationEvent(new NotificationEventArgs(string.Format("Unknown Message Received to Command Queue Service Host. Message: {0}", e.Message), OutputLevel.Warning));
        }

        private static void ServiceEvent_ClientClosingEvent(object sender, EventArgs e)
        {
            //InvokeNotificationEvent(new NotificationEventArgs("Client closing..."));
        }

        private static void ServiceEvent_ClientClosedEvent(object sender, EventArgs e)
        {
            //InvokeNotificationEvent(new NotificationEventArgs("Client closed"));
        }

        private static void ServiceEvent_SubscriptionStartedEvent(object sender, ServiceMessageBase<T>.SubscriptionStartedEventArgs e)
        {
            InvokeNotificationEvent(new NotificationEventArgs(string.Format("Subscription Started. (There are {0} subscribers.)", e.SubscriberCount)));
        }

        private static void ServiceEvent_SubscriptionStoppedEvent(object sender, ServiceMessageBase<T>.SubscriptionStoppedEventArgs e)
        {
            InvokeNotificationEvent(new NotificationEventArgs(string.Format("Subscription Stopped. (There are {0} subscribers.)", e.SubscriberCount)));
        }

        void ServiceHandler_SubscriptionCheckedEvent(object sender, ServiceMessageBase<T>.SubscriptionCheckedEventArgs e)
        {
            InvokeNotificationEvent(new NotificationEventArgs(string.Format("Subscription Checked. (There are {0} subscribers.)", e.SubscriberCount)));
        }

        private static void NevadaServiceEvent_RequestEvent(object sender, ServiceMessageBase<T>.RequestEventArgs e)
        {
            if (e.Success)
                InvokeNotificationEvent(new NotificationEventArgs(string.Format("Event {0} {{ {1} {2} ms }}", e.Method, "Success in", e.Elapsed.TotalMilliseconds)));
            else
            {
                var message = new StringBuilder();
                message.AppendLine(string.Format("Event {0}", e.Method));
                message.AppendLine("{");
                message.AppendLine(string.Format("\tFailed after {0} ms", e.Elapsed.TotalMilliseconds));
                message.AppendLine(string.Format("\t{0}", e.Exception.Message));
                message.AppendLine(string.Format("\t@ {0}", e.Exception.StackTrace));
                message.AppendLine("}");
                //var message = string.Format("Event {0} {{ {1} {2} ms }}", e.Method, "Failed after", e.Elapsed.TotalMilliseconds);
                InvokeNotificationEvent(new NotificationEventArgs(message.ToString(), OutputLevel.Error));
            }
        }

        private static void NevadaServiceCommand_CommandExecutedEvent(object sender, ServiceCommandBase.CommandExecutedEventArgs e)
        {
            if (e.Success)
                InvokeNotificationEvent(new NotificationEventArgs(string.Format("Event {0} {{ {1} {2} ms }}", e.Method, "Success in", e.Elapsed.TotalMilliseconds)));
            else
            {
                var message = new StringBuilder();
                message.AppendLine(string.Format("Command {0}", e.Method));
                message.AppendLine("{");
                message.AppendLine(string.Format("\tFailed after {0} ms", e.Elapsed.TotalMilliseconds));
                message.AppendLine(string.Format("\t{0}", e.Exception.Message));
                message.AppendLine(string.Format("\t@ {0}", e.Exception.StackTrace));
                message.AppendLine("}");
                //var message = string.Format("Event {0} {{ {1} {2} ms }}", e.Method, "Failed after", e.Elapsed.TotalMilliseconds);
                InvokeNotificationEvent(new NotificationEventArgs(message.ToString(), OutputLevel.Error));
            }
            //InvokeNotificationEvent(new NotificationEventArgs(string.Format("Command {0} {{ {1} {2} ms }}", e.Method, e.Success ? "Success in" : "Failed after", e.Elapsed.TotalMilliseconds)));
        }

        #endregion
    }
}