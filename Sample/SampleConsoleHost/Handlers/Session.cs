using System;
using Tharga.Toolkit.Storage;

namespace SampleConsoleHost.Handlers
{
    public class Session : ISession
    {
        public Guid SessionToken { get; internal set; }
        public Guid RealmId { get; internal set; }
    }
}