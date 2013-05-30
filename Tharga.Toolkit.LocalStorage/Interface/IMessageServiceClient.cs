using System;
using System.ServiceModel;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.LocalStorage.Interface
{
    public interface IMessageServiceClient
    {
        void Abort();
        void Close();
        void Sync<T>(Guid sessionToken, DateTime? serverStoreTime);

        void StartSubscription(StartSubscriptionRequest request);
        void StopSubscription(StopSubscriptionRequest request);
        void CheckSubscription(CheckSubscriptionRequest request);
        void CreateSession(CreateSessionRequest request);
        void EndSession(EndSessionRequest request);
        void Execute(Guid sessionToken, object command);

        IDuplexContextChannel InnerDuplexChannel { get; }
        IClientChannel InnerChannel { get; }
        CommunicationState State { get; }
    }
}