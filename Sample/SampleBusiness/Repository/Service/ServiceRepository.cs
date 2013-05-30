using SampleBusiness.MessageReference;
using SampleConsoleHost;
using Tharga.Toolkit.LocalStorage;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.LocalStorage.Repository;
using Tharga.Toolkit.Storage;

namespace SampleBusiness.Repository.Service
{
    public abstract class ServiceRepository<TEntity> : ServiceRepositoryBase<TEntity>
        where TEntity : IEntity
    {
        protected override ServiceCommandClient CreateCommandClient()
        {
            //TODO: Same code on two places
            KnownCommandTypesProvider.KnownCommandTypesProviderLoader = TypesProviderLoader.KnownCommandTypesProviderLoader;
            return ServiceCommandClientCreator.GetServiceCommandClient();
        }

        protected sealed override IMessageServiceClient GetSyncEventClient()
        {
            //TODO: Same code on two places
            return ServiceMessageClient.GetSyncEventClient(this);
        }
    }
}