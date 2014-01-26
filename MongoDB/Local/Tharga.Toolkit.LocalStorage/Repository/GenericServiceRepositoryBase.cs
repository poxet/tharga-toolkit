using System;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.LocalStorage.Utility;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.LocalStorage.Repository
{
    public abstract class GenericServiceRepositoryBase<TEntity> : ServiceRepositoryBase<TEntity>
        where TEntity : IEntity
    {
        private readonly Func<TEntity, object> _entityToDtoConverter;

        protected GenericServiceRepositoryBase(Func<TEntity, object> entityToDtoConverter)
        {
            _entityToDtoConverter = entityToDtoConverter;
        }

        public override void Save(Guid sessionToken, TEntity item, bool notifySubscribers)
        {
            var command = new SaveCommand { Item = _entityToDtoConverter(item), NotifySubscribers = notifySubscribers, TypeName = typeof(TEntity).Name };
            WcfShell.Execute(CreateCommandClient, client => client.Execute(sessionToken, command));
        }

        public override void Delete(Guid sessionToken, Guid id)
        {
            WcfShell.Execute(CreateCommandClient, client => client.Execute(sessionToken, new DeleteCommand { Id = id, TypeName = typeof(TEntity).Name }));
        }
    }
}