//using System;
//using SampleConsoleHost.Business;
//using SampleDataTransfer.Command;
//using Tharga.Toolkit.ServerStorage.CommandBase;
//using Tharga.Toolkit.ServerStorage.Interface;

//namespace SampleConsoleHost.Handlers
//{
//    public class SyncProductCommandHandler :SyncCommandHandlerBase,  IMessageHandler<SyncProductCommand>
//    {
//        public void Handle(Guid realmId, SyncProductCommand command, IServiceMessageBase serviceMessage)
//        {
//            Handle(realmId, new ProductBusiness(), command.SyncRequest.ServerStoreTime, serviceMessage);
//        }
//    }
//}