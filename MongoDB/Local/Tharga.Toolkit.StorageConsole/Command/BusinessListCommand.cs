using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;
using Tharga.Toolkit.StorageConsole.Utility;

namespace Tharga.Toolkit.StorageConsole.Command
{
    internal sealed class BusinessListCommand<THandler, TEntity> : ActionCommand<TEntity>
        where THandler : BusinessBase<TEntity>
        where TEntity : IEntity
    {
        private readonly THandler _instance;

        public BusinessListCommand(IConsole console, THandler instance)
            : base("list", "Lists all items")
        {
            _instance = instance;
            OutputAction = OutputEntity;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            throw new NotImplementedException();
            //foreach (var item in await GetAllAsync())
            //    Output(OutputAction.Invoke(item), item.GetColor(), true);

            //return true;
        }

        private string OutputEntity(TEntity entity)
        {
            return string.Format("{0}", entity.Id);
        }

        private async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _instance.GetAllAsync();
        }
    }
}