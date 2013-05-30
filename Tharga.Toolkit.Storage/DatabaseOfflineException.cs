using System;

namespace Tharga.Toolkit.Storage
{
    public sealed class DatabaseOfflineException : SystemException
    {
        public DatabaseOfflineException(string message, Exception innerException, string databaseName)
            : base(message, innerException)
        {
            Data.Add("DatabaseName", databaseName);
        }
    }
}