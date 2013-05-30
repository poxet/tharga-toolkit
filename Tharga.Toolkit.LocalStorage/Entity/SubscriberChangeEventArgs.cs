using System;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public class SubscriberChangeEventArgs : EventArgs
    {
        public int SubscriberCount { get; private set; }
        public string ClientAddress { get; private set; }
        public string ServerAddress { get; private set; }

        public SubscriberChangeEventArgs(int subscriberCount, string clientAddress, string serverAddress)
        {
            SubscriberCount = subscriberCount;
            ClientAddress = clientAddress;
            ServerAddress = serverAddress;
        }
    }
}