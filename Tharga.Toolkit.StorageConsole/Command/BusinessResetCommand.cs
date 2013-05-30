using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.StorageConsole.Command
{
    class BusinessResetCommand<THandler, TEntity> : ActionCommandBase
        where THandler : BusinessBase<TEntity>
        where TEntity : IEntity
    {
        private readonly THandler _instance;

        public BusinessResetCommand(THandler instance)
            : base("reset", "Resets local repository")
        {
            _instance = instance;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            await _instance.ResetAsync();
            return true;
        }        
    }
}