using System;

namespace Tharga.Toolkit.Console.Command.Base
{
    public class ServerConsole : SystemConsoleBase
    {
        protected override void WriteLine(string value)
        {
            var output = string.Format("{0} {1}: {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), value);
            System.Console.WriteLine(output);
        }
    }
}