using System;
using System.Collections.Generic;
using System.Linq;

namespace Tharga.Toolkit.LocalStorage.Business
{
    abstract class BusinessBaseHelper
    {
        internal static readonly object SyncRoot = new object();
        private readonly static List<Type> Synced = new List<Type>();

        internal static bool IsSynced<TEntity>()
        {
            return Synced.Any(x => x == typeof (TEntity));
        }

        internal static void SetSynced<TEntity>()
        {
            if (!IsSynced<TEntity>())
                Synced.Add(typeof (TEntity));
        }

        internal static void ClearSynced<TEntity>()
        {
            if (IsSynced<TEntity>())
                Synced.Remove(typeof(TEntity));
        }

        internal static void ClearAllSynced()
        {
            Synced.Clear();
        }
    }
}