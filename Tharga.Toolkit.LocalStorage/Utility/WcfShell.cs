using System;
using System.ServiceModel;

namespace Tharga.Toolkit.LocalStorage.Utility
{
    public static class WcfShell
    {
        public static void Execute<T>(Func<T> clientCreator, Action<T> block)
            where T : ICommunicationObject
        {
            var client = clientCreator.Invoke();
            try
            {
                block(client);
                client.Close();
            }
            catch
            {
                client.Abort();
                throw;
            }
        }
    }
}
