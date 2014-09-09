using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Tharga.Toolkit.ServerStorage
{
    public class ErrorHandler<T> : IErrorHandler
    {
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            
        }

        public bool HandleError(Exception error)
        {
            ServiceHandler<T>.InvokeNotificationEvent(new ServiceHandler<T>.NotificationEventArgs(string.Format("Command Queue Service Host Faulted. Exception: {0} (Inner: {1})", error.Message, error.InnerException != null ? error.InnerException.Message : "N/A"), ServiceHandler<T>.OutputLevel.Error));
            return false;
        }
    }
}