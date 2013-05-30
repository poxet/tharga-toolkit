using System;
using SampleDataTransfer.Command;
using SampleDataTransfer.Entities;
using Tharga.Toolkit.ServerStorage.Interface;

namespace SampleConsoleHost.Handlers
{
    public class MoveCustomerCommandHandler : IMessageHandler<MoveCustomerCommand>
    {
        public void Handle(Guid realmId, MoveCustomerCommand command, IServiceMessageBase serviceMessage)
        {
            //TODO: Perform the action in question
            //Thread.Sleep(5000);

            var response = new MoveCustomerResponse {Message = "Some message"};
            serviceMessage.NotifyExecuteComplete(response); //sends a response message back to the caller only
            //ServiceMessage.NotifyAllExecuteComplete(command.CommandToken, response); //send a response to everyone
        }
    }
}