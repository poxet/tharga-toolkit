using System;

namespace Tharga.Toolkit.Storage
{
    public interface IOnSavedRequest
    {
        DateTime? PreviousServerStoreTime { get; set; }
    }
}