using System;
using System.ServiceModel;

namespace Tharga.Toolkit.Storage
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [ServiceContract(ConfigurationName = "CommandReference.IServiceCommand", SessionMode = SessionMode.NotAllowed)]
    [ServiceKnownType("GetKnownTypes", typeof(KnownCommandTypesProvider))]
    public interface IServiceCommand
    {
        [OperationContract(IsOneWay = true, Action = "http://tempuri.org/IServiceCommand/CheckSubscription")]
        void CheckSubscription(CheckSubscriptionRequest request);

        [OperationContract(IsOneWay = true, Action = "http://tempuri.org/IServiceCommand/Execute")]
        void Execute(Guid sessionToken, object command);
    }
}