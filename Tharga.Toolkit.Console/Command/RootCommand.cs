using System;
using Tharga.Toolkit.Console.Command.Base;

namespace Tharga.Toolkit.Console.Command
{
    public sealed class RootCommand : RootCommandBase
    {
        internal RootCommand(IConsole console)
            : this(console, null)
        {

        }

        internal RootCommand(IConsole console, Action stopAction)
            : base(console, stopAction)
        {

        }
    }
}