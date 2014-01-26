using System;

namespace Tharga.Toolkit.LocalStorage.Interface
{
    public interface ISyncCompleteData
    {
        DateTime? LastServerStoreTime { get; }
        int EntityChangedCount { get; }
        int EntityDeletedCount { get; }
    }
}