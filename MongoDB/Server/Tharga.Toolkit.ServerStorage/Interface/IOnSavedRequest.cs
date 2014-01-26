using System;

namespace Tharga.Toolkit.ServerStorage.Interface
{
    public interface IOnSavedRequest
    {
        DateTime? PreviousServerStoreTime { get; set; }
    }
}