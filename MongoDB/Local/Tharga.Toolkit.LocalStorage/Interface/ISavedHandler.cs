using System;

namespace Tharga.Toolkit.LocalStorage.Interface
{
    public interface ISavedHandler<in TDto>
    {
        void Handle(Guid realmId, TDto dto, DateTime? previousServerStoreTime);
    }
}