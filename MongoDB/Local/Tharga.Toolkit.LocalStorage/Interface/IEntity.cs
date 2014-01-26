using Tharga.Toolkit.LocalStorage.Entity;
using Tharga.Toolkit.LocalStorage.Repository;

namespace Tharga.Toolkit.LocalStorage.Interface
{
    public interface IEntity : IId
    {
        StoreInfo StoreInfo { get; }
    }
}