using System;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public class SubscriptionStartedEventArgs : EventArgs
    {
        public Guid SubscriptionToken { get; private set; }
        public string ClientAddress { get; private set; }
        public string ServerAddress { get; private set; }

        public SubscriptionStartedEventArgs(Guid subscriptionToken, string clientAddress, string serverAddress)
        {
            SubscriptionToken = subscriptionToken;
            ClientAddress = clientAddress;
            ServerAddress = serverAddress;
        }
    }
}