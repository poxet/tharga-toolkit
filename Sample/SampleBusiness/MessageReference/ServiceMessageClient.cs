using System;
using System.ServiceModel;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.LocalStorage.Repository;
using Tharga.Toolkit.ServerStorage;
using Tharga.Toolkit.Storage;

namespace SampleBusiness.MessageReference
{
    public class ServiceMessageClient : ServiceMessageClientBase, IMessageServiceClient
    {
        private static ServiceMessageClient _serviceEventClient;

        public ServiceMessageClient(InstanceContext callbackInstance, string endpointConfigurationName) :
            base(callbackInstance, endpointConfigurationName)
        {
            
        }

        public override void Sync<T>(Guid sessionToken, DateTime? serverStoreTime)
        {
            var syncRequest = new SyncRequest { ServerStoreTime = serverStoreTime };

            switch (typeof(T).Name)
            {
                //case "IProductEntity":
                //    Execute(new SyncProductCommand { SyncRequest = syncRequest });
                //    break;
                //case "ICustomerEntity":
                //    Execute(new SyncCustomerCommand { SyncRequest = syncRequest });
                //    break;
                default:
                    var command = new SyncCommand {TypeName = typeof (T).Name, SyncRequest = syncRequest};
                    Execute(sessionToken, command);
                    break;
            }
        }

        //TODO: Does this method has to be static? Is this really the correct place for this method. Should it not be inside SubscriptionCallback?
        internal static IMessageServiceClient GetSyncEventClient<TEntity>(ServiceRepositoryBase<TEntity> serviceRepository)
            where TEntity : IEntity
        {
            if (_serviceEventClient != null && (_serviceEventClient.State == CommunicationState.Faulted || _serviceEventClient.State == CommunicationState.Closed))
                _serviceEventClient = null;

            if (_serviceEventClient == null)
            {
                var callback = new SubscriptionCallback();
                callback.SyncCompleteEvent += serviceRepository.OnSyncCompleteEvent;
                _serviceEventClient = ServiceCommandClientCreator.GetServiceMessageClient(new InstanceContext(callback));
            }
            return _serviceEventClient;
        }
    }
}