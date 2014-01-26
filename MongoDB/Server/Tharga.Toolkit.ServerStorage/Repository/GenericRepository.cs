using Tharga.Toolkit.ServerStorage.Interface;

namespace Tharga.Toolkit.ServerStorage.Repository
{
    public sealed class GenericRepository<TEntity> : RepositoryBase<TEntity>
        where TEntity : IOutputDto
    {
        public GenericRepository()
            : base(new MongoRepository())
        {

        }
    }
}