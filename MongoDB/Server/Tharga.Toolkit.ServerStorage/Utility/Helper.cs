using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Toolkit.ServerStorage.Interface;

namespace Tharga.Toolkit.ServerStorage.Utility
{
    public static class Helper
    {
        private static DateTime? LastServerStoreTime<TEntity>(this IList<TEntity> items)
            where TEntity : IOutputDto
        {
            return !items.Any() ? (DateTime?)null : items.Max(x => x.StoreInfo.ServerStoreTime != null ? x.StoreInfo.ServerStoreTime.Value : new DateTime());
        }
    }
}