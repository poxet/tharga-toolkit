using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Repository
{
    public class GenericLocalRepository<TEntity> : LocalRepositoryBase<TEntity>
        where TEntity : IEntity
    {

    }
}