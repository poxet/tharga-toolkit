using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Tharga.Toolkit.ServerStorage
{
    public class ErrorServiceBehavior<T> : IServiceBehavior
    {
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            var handler = new ErrorHandler<T>();
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
                dispatcher.ErrorHandlers.Add(handler);
        }
    }
}