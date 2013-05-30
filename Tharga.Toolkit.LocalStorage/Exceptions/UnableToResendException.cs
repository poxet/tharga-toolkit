using System;

namespace Tharga.Toolkit.LocalStorage.Exceptions
{
    sealed class UnableToResendException : SystemException
    {
        public UnableToResendException(int countInQueue, int maxQueueCount)
            : base("The queue seems to be offline. Resending more messages is pointless.")
        {
            Data.Add("CountInQueue", countInQueue);
            Data.Add("MaxQueueCount", maxQueueCount);
        }
    }
}
