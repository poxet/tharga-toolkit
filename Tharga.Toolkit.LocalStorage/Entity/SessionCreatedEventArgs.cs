using System;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public class SessionCreatedEventArgs : EventArgs
    {
        public Guid SessionToken { get; private set; }
        public Guid RealmId { get; private set; }

        internal SessionCreatedEventArgs(Guid sessionToken, Guid realmId)
        {
            SessionToken = sessionToken;
            RealmId = realmId;
        }
    }
}