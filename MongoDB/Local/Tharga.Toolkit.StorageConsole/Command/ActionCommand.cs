using System;
using Tharga.Toolkit.Console.Command.Base;

namespace Tharga.Toolkit.StorageConsole.Command
{
    abstract class ActionCommand<TEntity> : ActionCommandBase
    {
        protected Func<TEntity, string> OutputAction;

        protected ActionCommand(string name, string description) : base(name, description)
        {
        }

        protected ActionCommand(string[] names, string description) : base(names, description)
        {
        }

        public void SetEntityOutput(Func<TEntity, string> outputAction)
        {
            OutputAction = outputAction;
        }
    }
}