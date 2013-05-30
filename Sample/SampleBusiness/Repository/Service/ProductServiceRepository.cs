using System;
using SampleBusiness.Converter;
using SampleBusiness.Interface;
using SampleDataTransfer.Command;
using Tharga.Toolkit.LocalStorage.Utility;
using Tharga.Toolkit.Storage;

namespace SampleBusiness.Repository.Service
{
    public sealed class ProductServiceRepository : ServiceRepository<IProductEntity>
    {
        public override void Save(Guid sessionToken, IProductEntity item, bool notifySubscribers)
        {
            //NOTE: Use the generic command
            var command = new SaveCommand { Item = item.ToProductDto(), NotifySubscribers = notifySubscribers, TypeName = typeof(IProductEntity).Name };

            ////NOTE: Use the custom command
            //var command = new SaveProductCommand { Product = item.ToProductDto(), NotifySubscribers = notifySubscribers };

            WcfShell.Execute(CreateCommandClient, client => client.Execute(sessionToken, command));
        }

        public override void Delete(Guid sessionToken, Guid id)
        {
            //NOTE: Use the generic delete command
            WcfShell.Execute(CreateCommandClient, client => client.Execute(sessionToken, new DeleteCommand { Id = id, TypeName = typeof(IProductEntity).Name }));

            //NOTE: Use the custom delete command
            //WcfShell.Execute(CreateCommandClient, client => client.Execute(sessionToken, new DeleteProductCommand { Id = id }));
        }
    }
}