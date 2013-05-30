using System;
using System.Reflection;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.ServerStorage;
using Tharga.Toolkit.ServerStorage.Utility;
using Tharga.Toolkit.Storage;
using OutputLevel = Tharga.Toolkit.Console.Command.Base.OutputLevel;

namespace SampleConsoleHost
{
    internal static class Program
    {
        private static ServerConsole _output;

        private static void Main(string[] args)
        {
            _output = new ServerConsole();

            try
            {
                ServiceHandler<ICallback>.NotificationEvent += ServiceHandler_NotificationEvent;
                Bootstrapper.Assembly = Assembly.GetExecutingAssembly();
                KnownCommandTypesProvider.KnownCommandTypesProviderLoader = TypesProviderLoader.KnownCommandTypesProviderLoader;
                KnownCallbackTypesProvider.KnownCallbackTypesProviderLoader = TypesProviderLoader.KnownCallbackTypesProviderLoader;
                KnownMessageTypesProvider.KnownMessageTypesProviderLoader = TypesProviderLoader.KnownCommandTypesProviderLoader;
                var serviceHandler = new ServiceHandler<ICallback>(typeof(ServiceMessage), typeof(ServiceCommand));
                serviceHandler.Open();

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(true);

                serviceHandler.Close();
            }
            catch (Exception exception)
            {
                _output.WriteLine(string.Format("{0} (Inner: {1})", exception.Message, exception.InnerException != null ? exception.InnerException.Message : "N/A"), OutputLevel.Error);
                Console.ReadKey(true);
            }
        }

        private static void ServiceHandler_NotificationEvent(object sender, NotificationEventArgs e)
        {
            switch (e.OutputLevel)
            {
                case Tharga.Toolkit.ServerStorage.OutputLevel.Default:
                    _output.WriteLine(e.Message, OutputLevel.Default);
                    break;
                case Tharga.Toolkit.ServerStorage.OutputLevel.Information:
                    _output.WriteLine(e.Message, OutputLevel.Information);
                    break;
                case Tharga.Toolkit.ServerStorage.OutputLevel.Warning:
                    _output.WriteLine(e.Message, OutputLevel.Warning);
                    break;
                case Tharga.Toolkit.ServerStorage.OutputLevel.Error:
                    _output.WriteLine(e.Message, OutputLevel.Error);
                    break;
                default:
                    _output.WriteLine(string.Format("Unknown level {0}. {1}", e.OutputLevel, e.Message), OutputLevel.Error);
                    break;
            }
        }
    }
}
