using MongoDB.Bson.Serialization;
using SampleBusiness;
using SampleBusiness.Business;
using SampleBusiness.Entities;
using SampleBusiness.Interface;
using SampleBusiness.Repository.Local;
using SampleBusiness.Repository.Service;
using SampleConsoleClient.Command;
using Tharga.Toolkit.Console;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.StorageConsole.Command;

namespace SampleConsoleClient
{
    static class Program
    {
        static void Main(string[] args)
        {
            var productServiceRepositoryInstance = new ProductServiceRepository();
            var subscriptionHandler = new SubscriptionHandler(new SubscriptionServiceRepository());
            var command = new RootCommand(subscriptionHandler, productServiceRepositoryInstance.OutgoingCommandQueueName);

            command.UnregisterCommand("user");
            var realmBusiness = new RealmBusiness(new RealmLocalRepository(), new RealmServiceRepository(), subscriptionHandler);
            command.RegisterCommand(new Command.UserCommand(new UserBusiness(new UserLocalRepository(), new UserServiceRepository(), subscriptionHandler), realmBusiness));
            command.RegisterCommand(new RealmCommand(realmBusiness));

            //BsonClassMap.LookupClassMap(typeof(CustomerEntity));
            //BsonClassMap.RegisterClassMap<CustomerEntity>();

            command.RegisterCommand(new ProductCommand(new ProductBusiness(new ProductLocalRepository(), productServiceRepositoryInstance, subscriptionHandler)));
            //command.RegisterCommand(new CustomerCommand(new CustomerBusiness(new CustomerLocalRepository(), new GenericServiceRepository<ICustomerEntity>(SampleBusiness.Converter.Converter.ToCustomerDto), subscriptionHandler)));
            //command.RegisterCommand(new CustomerCommand(new CustomerBusiness(new GenericLocalRepository<ICustomerEntity>(), new GenericServiceRepository<ICustomerEntity>(SampleBusiness.Converter.Converter.ToCustomerDto), subscriptionHandler)));
            command.RegisterCommand(new CustomerCommand(new GenericRealmBusiness<ICustomerEntity>(SampleBusiness.Converter.Converter.ToCustomerDto, subscriptionHandler, typeof(CustomerEntity))));

            new CommandEngine(command).Run(args);
        }
    }
}
