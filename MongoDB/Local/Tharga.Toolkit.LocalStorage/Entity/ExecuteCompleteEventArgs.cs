using System;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    public class ExecuteCompleteEventArgs : EventArgs
    {
        public object Response { get; private set; }

        public ExecuteCompleteEventArgs(object response)
        {
            Response = response;
        }
    }
}