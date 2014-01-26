using System;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.LocalStorage
{
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public class ServiceCommandClient : System.ServiceModel.ClientBase<IServiceCommand>, IServiceCommand, IServiceCommandClient
    {
        public ServiceCommandClient()
        {

        }

        public ServiceCommandClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {

        }

        public ServiceCommandClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {

        }

        public ServiceCommandClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {

        }

        public ServiceCommandClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {

        }

        public void CheckSubscription(CheckSubscriptionRequest request)
        {
            Channel.CheckSubscription(request);
        }

        public void Execute(Guid sessionToken, object command)
        {
            Channel.Execute(sessionToken, command);
        }

        public void CheckSubscription(Guid subscriptionToken)
        {
            CheckSubscription(new CheckSubscriptionRequest { SubscriptionToken = subscriptionToken });
        }
    }
}