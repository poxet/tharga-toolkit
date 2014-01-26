using System.Threading.Tasks;
using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

namespace SampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = new RootCommand();
            command.RegisterCommand(new SomeContainerCommand());
            new CommandEngine(command).Run(args);
        }
    }

    class SomeContainerCommand : ContainerCommandBase
    {
        public SomeContainerCommand() 
            : base("some")
        {
            RegisterCommand(new SomeListCommand());
        }
    }

    class SomeListCommand : ActionCommandBase
    {
        public SomeListCommand() 
            : base("list", "lists some information")
        {

        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            for (var i = 0; i < 5; i++)
                OutputInformation("Some data {0}", i);
            return true;
        }
    }
}
