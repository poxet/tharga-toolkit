using System;
using Tharga.Toolkit.Console.Command.Base;

namespace Tharga.Toolkit.StorageConsole.Command
{
    abstract class ActionCommand<TEntity> : ActionCommandBase
    {
        protected Func<TEntity, string> OutputAction;

        protected ActionCommand(IConsole console, string name, string description)
            : this(console,name,description,null)
        {

        }

        private ActionCommand(IConsole console, string name, string description, string helpText)
            : base(console, name, description, helpText)
        {

        }

        public void SetEntityOutput(Func<TEntity, string> outputAction)
        {
            OutputAction = outputAction;
        }
    }
}