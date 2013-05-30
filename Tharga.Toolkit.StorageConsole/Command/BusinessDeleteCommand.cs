using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.StorageConsole.Command
{
    class BusinessDeleteCommand<THandler, TEntity> : ActionCommandBase
        where THandler : BusinessBase<TEntity> 
        where TEntity : IEntity
    {
        private readonly THandler _instance;

        public BusinessDeleteCommand(THandler instance)
            : base("delete", "delete a single entity")
        {
            _instance = instance;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var id = QueryParam<Guid>("Id", GetParam(paramList, 0), GetItemList());

            await _instance.DeleteAsync(id);
            return true;
        }

        private Func<List<KeyValuePair<string, string>>> GetItemList()
        {
            return () =>
                {
                    var list = _instance.GetAllAsync().Result;
                    return list.Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Id.ToString())).ToList();
                };
        }
    }
}