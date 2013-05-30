using System;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public class CheckOnlineStatusEventArgs : EventArgs
    {
        public int RetryCount { get; private set; }
        public string ClientAddress { get; private set; }
        public string ServerAddress { get; private set; }

        public CheckOnlineStatusEventArgs(int retryCount, string clientAddress, string serverAddress)
        {
            RetryCount = retryCount;
            ClientAddress = clientAddress;
            ServerAddress = serverAddress;
        }
    }
}