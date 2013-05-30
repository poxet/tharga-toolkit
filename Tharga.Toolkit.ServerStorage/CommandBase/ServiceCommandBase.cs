using System;
using Tharga.Toolkit.ServerStorage.Utility;

namespace Tharga.Toolkit.ServerStorage.CommandBase
{
    public abstract class ServiceCommandBase
    {
        public class CommandExecutedEventArgs : EventArgs
        {
            public string Method { get; private set; }
            public TimeSpan Elapsed { get; private set; }
            public Exception Exception { get; private set; }
            public bool Success { get { return Exception == null; } }

            internal CommandExecutedEventArgs(string method, TimeSpan elapsed, Exception exception = null)
            {
                Method = method;
                Elapsed = elapsed;
                Exception = exception;
            }
        }

        public static event EventHandler<CommandExecutedEventArgs> CommandExecutedEvent;

        private static void InvokeCommandExecutedEvent(CommandExecutedEventArgs e)
        {
            var handler = CommandExecutedEvent;
            if (handler != null) handler(null, e);
        }

        protected void ExecuteCommand(string method, Action block)
        {
            var sw = new StopWatch();

            try
            {
                block();

                InvokeCommandExecutedEvent(new CommandExecutedEventArgs(method, sw.Elapsed));
            }
            catch (Exception exception)
            {
                InvokeCommandExecutedEvent(new CommandExecutedEventArgs(method, sw.Elapsed, exception));
            }
        }
    }
}