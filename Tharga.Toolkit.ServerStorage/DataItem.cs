using System;
using Tharga.Toolkit.ServerStorage.Interface;

namespace Tharga.Toolkit.ServerStorage
{
    public class DataItem<T> where T : IDto
    {
        public Guid Id { get; private set; }
        public Guid RealmId { get; private set; }
        public T Item { get; set; }

        private DataItem()
        {

        }

        public static DataItem<T> Create(Guid realmId, T item)
        {
            var mi = new DataItem<T>
                         {
                             RealmId = realmId,
                             Id = item.Id,
                             Item = item,
                         };

            return mi;
        }
    }
}