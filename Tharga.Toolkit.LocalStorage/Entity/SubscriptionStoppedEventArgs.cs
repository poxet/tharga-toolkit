using System;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public class SubscriptionStoppedEventArgs : EventArgs
    {
        public enum StopReason { TriggeredByClient, TriggeredByServer, ConnectionLost }

        public StopReason Reason { get; private set; }
        public string ClientAddress { get; private set; }
        public string ServerAddress { get; private set; }

        public SubscriptionStoppedEventArgs(StopReason reason, string clientAddress, string serverAddress)
        {
            Reason = reason;
            ClientAddress = clientAddress;
            ServerAddress = serverAddress;
        }
    }
}