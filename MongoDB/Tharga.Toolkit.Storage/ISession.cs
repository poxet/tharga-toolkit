using System;

namespace Tharga.Toolkit.Storage
{
    public interface ISession
    {
        Guid SessionToken { get; }
        Guid RealmId { get; }
    }
}