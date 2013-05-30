using System.Reflection;
using System.ServiceModel;
using SampleConsoleHost;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.LocalStorage.Utility;
using Tharga.Toolkit.ServerStorage;
using Tharga.Toolkit.Storage;

namespace SampleBusiness
{
    public class SubscriptionServiceRepository : SubscriptionServiceRepositoryBase
    {
        public SubscriptionServiceRepository()
        {
            Bootstrapper.Assembly = Assembly.GetExecutingAssembly();
            KnownCallbackTypesProvider.KnownCallbackTypesProviderLoader = TypesProviderLoader.KnownCallbackTypesProviderLoader;
            KnownMessageTypesProvider.KnownMessageTypesProviderLoader = TypesProviderLoader.KnownCommandTypesProviderLoader;            
        }

        protected override IServiceCommandClient GetQueueClient()
        {
            return ServiceCommandClientCreator.GetServiceCommandClient();
        }

        protected override ISubscriptionServiceClient GetServiceClient()
        {
            return new SubscriptionCallback();
        }

        protected override IMessageServiceClient CreateEventServiceClient(InstanceContext callbackInstance)
        {
            return ServiceCommandClientCreator.GetServiceMessageClient(callbackInstance);
        }
    }
}
