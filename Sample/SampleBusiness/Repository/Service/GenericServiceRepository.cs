using System;
using SampleBusiness.MessageReference;
using SampleConsoleHost;
using Tharga.Toolkit.LocalStorage;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.LocalStorage.Repository;
using Tharga.Toolkit.Storage;

namespace SampleBusiness.Repository.Service
{
    public sealed class GenericServiceRepository<TEntity> : GenericServiceRepositoryBase<TEntity>
        where TEntity : IEntity
    {
        public GenericServiceRepository(Func<TEntity, object> entityToDtoConverter)
            : base(entityToDtoConverter)
        {
            
        }

        protected override ServiceCommandClient CreateCommandClient()
        {
            //TODO: Same code on two places (this is same as in ServiceRepository)
            KnownCommandTypesProvider.KnownCommandTypesProviderLoader = TypesProviderLoader.KnownCommandTypesProviderLoader;
            return ServiceCommandClientCreator.GetServiceCommandClient();
        }

        protected override IMessageServiceClient GetSyncEventClient()
        {
            //TODO: Same code on two places (this is same as in ServiceRepository)
            return ServiceMessageClient.GetSyncEventClient(this);
        }
    }
}