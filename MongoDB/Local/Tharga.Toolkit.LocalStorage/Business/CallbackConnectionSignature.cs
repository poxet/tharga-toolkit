using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Business
{
    //public class MyErrorHandler : IErrorHandler
    //{
    //    public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
    //    {

    //    }

    //    public bool HandleError(Exception error)
    //    {
    //        Console.WriteLine(error.Message);
    //        //ServiceHandler<T>.InvokeNotificationEvent(new ServiceHandler<T>.NotificationEventArgs(string.Format("Command Queue Service Host Faulted."), ServiceHandler<T>.OutputLevel.Error));
    //        return false;
    //    }
    //}

    //class MyErrorBehaviour : IExtension<IContextChannel>, IServiceBehavior
    //{
    //    public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    //    {
            
    //    }

    //    public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
    //    {
            
    //    }

    //    public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    //    {
    //        var handler = new MyErrorHandler();
    //        foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
    //            dispatcher.ErrorHandlers.Add(handler);
    //    }

    //    public void Attach(IContextChannel owner)
    //    {
            
    //    }

    //    public void Detach(IContextChannel owner)
    //    {
            
    //    }
    //}

    class CallbackConnectionSignature
    {
        public ISubscriptionServiceClient Client { get; private set; }
        public InstanceContext CallbackInstance { get; private set; }
        public IMessageServiceClient ServiceEventClient { get; private set; }

        #region Event

        public class ConnectionClosedEventArgs : EventArgs
        {
            public string ClientAddress { get; private set; }
            public string ServerAddress { get; private set; }

            public ConnectionClosedEventArgs(string clientAddress, string serverAddress)
            {
                ClientAddress = clientAddress;
                ServerAddress = serverAddress;
            }
        }

        public event EventHandler<ConnectionClosedEventArgs> ConnectionClosedEvent;

        private void InvokeConnectionClosedEvent(ConnectionClosedEventArgs e)
        {
            var handler = ConnectionClosedEvent;
            if (handler != null) handler(this, e);
        }

        #endregion

        public CallbackConnectionSignature(ISubscriptionServiceClient client, InstanceContext callbackInstance, IMessageServiceClient serviceEventClient)
        {
            Client = client;
            CallbackInstance = callbackInstance;
            ServiceEventClient = serviceEventClient;

            CallbackInstance.Faulted += CallbackInstance_Faulted;

            ServiceEventClient.InnerChannel.Faulted += InnerChannel_Faulted;            
            ServiceEventClient.InnerChannel.UnknownMessageReceived += InnerChannel_UnknownMessageReceived;

            ServiceEventClient.InnerDuplexChannel.Faulted += InnerDuplexChannel_Faulted;
            //ServiceEventClient.InnerDuplexChannel.Extensions.Add(new MyErrorBehaviour());
            //ServiceEventClient.InnerDuplexChannel.CallbackInstance.Extensions
        }

        void InnerDuplexChannel_Faulted(object sender, EventArgs e)
        {
            //TODO: Need to find ther real reason for the connection loss.
            //one reason could be that the message size is too large. That should be logged!
            InvokeConnectionClosedEvent(new ConnectionClosedEventArgs("Unknown_pos7", "Unknown_pos8"));
            //NOTE: Issue.Register(string.Format("InnerDuplexChannel_Faulted"), false, Tharga.Support.Client.Base.Issue.IssueLevel.Warning);
        }

        void InnerChannel_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            //NOTE: Issue.Register(string.Format("InnerChannel_UnknownMessageReceived: {0}", e.Message), false, Tharga.Support.Client.Base.Issue.IssueLevel.Warning);
        }

        void InnerChannel_Faulted(object sender, EventArgs e)
        {
            //Server has closed
            //NOTE: Issue.Register(string.Format("InnerChannel_Faulted"), false, Tharga.Support.Client.Base.Issue.IssueLevel.Warning);
        }

        void CallbackInstance_Faulted(object sender, EventArgs e)
        {
            //NOTE: Issue.Register(string.Format("CallbackInstance_Faulted"), false, Support.Client.Base.Issue.IssueLevel.Warning);
        }
    }
}