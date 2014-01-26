using System;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public class SessionEndedEventArgs : EventArgs
    {
        public Guid SessionToken { get; private set; }

        internal SessionEndedEventArgs(Guid sessionToken)
        {
            SessionToken = sessionToken;
        }        
    }
}