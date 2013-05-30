using System;

namespace Tharga.Toolkit.ServerStorage.Interface
{
    public interface ICommandHandler<in TCommand>
    {
        void Handle(Guid realmId, TCommand command);
    }
}