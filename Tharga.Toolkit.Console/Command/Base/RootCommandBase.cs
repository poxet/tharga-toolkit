using System;
using System.Threading.Tasks;

namespace Tharga.Toolkit.Console.Command.Base
{
    public abstract class RootCommandBase : ContainerCommandBase
    {
        internal RootCommandBase(IConsole console, Action stopAction)
            : base(console, "root")
        {
            RegisterCommand(new ExitCommand(_console, stopAction));
            RegisterCommand(new ClearCommand(_console));
            RegisterCommand(new ExecuteCommand(_console, this));
        }

        protected internal virtual void SetStopAction(Action stopAction)
        {
            var exitCommand = GetCommand("exit");

            if (exitCommand is ExitCommand)
                ((ExitCommand)GetCommand("exit")).SetStopAction(stopAction);
            else
                throw new ArgumentOutOfRangeException(string.Format("Unknown type for exit command. {0}", exitCommand.GetType()));
        }

        public sealed override async Task<bool> InvokeAsync(string paramList)
        {
            return await GetHelpCommand().InvokeAsync(paramList);
        }

        public bool ExecuteCommand(string entry)
        {
            var success = false;

            try
            {
                string subCommand;
                var command = GetSubCommand(entry, out subCommand);
                if (command != null)
                {
                    var task = command.InvokeWithCanExecuteCheckAsync(subCommand);
                    task.Wait();
                    success = task.Result;
                }
                else
                    OutputError("Invalid command {0}", entry);
            }
            catch (SystemException exception)
            {
                OutputError(exception.Message);
            }
            catch (AggregateException exception)
            {
                foreach (var exp in exception.InnerExceptions)
                    OutputError(exp.Message);
            }
            catch (Exception exception)
            {
                OutputError(exception.Message);
                OutputInformation("Terminating application...");
                throw;
            }

            //if (_commandMode && !success)
            //{
            //    _rootCommand.OutputError("Terminating command chain.");
            //    return false;
            //}
            return success;
        }

    }
}