using System;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.ServerStorage.Interface
{
    public class Session : ISession
    {
        public Guid SessionToken { get; private set; }
        public Guid RealmId { get; private set; }

        public Session(Guid sessionToken, Guid realmId)
        {
            if (sessionToken == Guid.Empty) throw new ArgumentException("Empty guid is not valid for sessionToken.");
            if (realmId == Guid.Empty) throw new ArgumentException("Empty guid is not valid for realmId.");

            SessionToken = sessionToken;
            RealmId = realmId;
        }
    }

    public interface ICreateSessionHandler<in TCommand>
        where TCommand : CreateSessionRequest
    {
        ISession Handle(TCommand command);
    }

    public interface IEndSessionHandler<in TCommand>
        where TCommand : EndSessionRequest
    {
        ISession Handle(TCommand command);
    }

    public interface IMessageHandler<in TCommand>
    {
        void Handle(Guid realmId, TCommand command, IServiceMessageBase serviceMessage);
    }
}