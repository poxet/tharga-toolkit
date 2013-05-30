using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.StorageConsole.Command
{
    class BusinessSyncCommand<THandler, TEntity> : ActionCommandBase
        where THandler : BusinessBase<TEntity> 
        where TEntity : IEntity
    {
        private readonly THandler _instance;

        public BusinessSyncCommand(THandler instance)
            : base("sync", "Sync local repository with server")
        {
            _instance = instance;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            await _instance.SyncAsync(SyncMode.Always);
            return true;
        }
    }
}