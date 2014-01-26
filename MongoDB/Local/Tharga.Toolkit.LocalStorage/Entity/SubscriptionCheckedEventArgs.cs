using System;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public class SubscriptionCheckedEventArgs : EventArgs
    {
        public enum EMethod { Command, Message }

        public bool Restarted { get; private set; }
        public string ClientAddress { get; private set; }
        public string ServerAddress { get; private set; }
        public EMethod Method { get; private set; }

        public SubscriptionCheckedEventArgs(bool restarted, string clientAddress, string serverAddress, EMethod method)
        {
            Restarted = restarted;
            ClientAddress = clientAddress;
            ServerAddress = serverAddress;
            Method = method;
        }
    }
}