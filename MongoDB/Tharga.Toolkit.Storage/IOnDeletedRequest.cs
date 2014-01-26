using System;

namespace Tharga.Toolkit.Storage
{
    public interface IOnDeletedRequest
    {
        DateTime? PreviousServerStoreTime { get; set; }
    }
}