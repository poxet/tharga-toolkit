using System;
using System.ServiceModel;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.ServerStorage
{
    [ServiceContract(ConfigurationName = "MessageReference.IServiceMessage", CallbackContract = typeof(ICallback), SessionMode = SessionMode.Required)]
    [ServiceKnownType("GetKnownTypes", typeof(KnownMessageTypesProvider))]
    public interface IServiceMessage
    {
        [OperationContract(IsOneWay = true)]
        void StartSubscription(StartSubscriptionRequest request);

        [OperationContract(IsOneWay = true)]
        void StopSubscription(StopSubscriptionRequest request);

        [OperationContract(IsOneWay = true)]
        void CheckSubscription(CheckSubscriptionRequest request);

        [OperationContract(IsOneWay = true)]
        void CreateSession(CreateSessionRequest request);

        [OperationContract(IsOneWay = true)]
        void EndSession(EndSessionRequest request);

        [OperationContract(IsOneWay = true)]
        [ServiceKnownType("GetKnownTypes", typeof(KnownCallbackTypesProvider))]
        void Execute(Guid sessionToken, object command);
    }
}