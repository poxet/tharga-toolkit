using System.ServiceModel;

namespace Tharga.Toolkit.Storage
{
    [ServiceKnownType("GetKnownTypes", typeof(KnownCallbackTypesProvider))]
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void OnSubscriptionStarted(OnSubscriptionStartedResponse response);

        [OperationContract(IsOneWay = true)]
        void OnSubscriptionStopped(OnSubscriptionStoppedResponse response);

        [OperationContract(IsOneWay = true)]
        void OnSubscriptionChecked(OnSubscriptionCheckedResponse response);

        [OperationContract(IsOneWay = true)]
        void OnSubscriberChange(OnSubscriberChangeResponse response);

        [OperationContract(IsOneWay = true)]
        void OnSessionCreated(OnSessionCreatedResponse response);

        [OperationContract(IsOneWay = true)]
        void OnSessionEnded(OnSessionEndedResponse response);

        [OperationContract(IsOneWay = true)]
        void OnSync(OnSyncResponse response);

        [OperationContract(IsOneWay = true)]
        void OnSaved(OnSavedResponse response);

        [OperationContract(IsOneWay = true)]
        void OnDeleted(OnDeletedResponse response);

        [OperationContract(IsOneWay = true)]
        void OnExecute(object response);
    }
}