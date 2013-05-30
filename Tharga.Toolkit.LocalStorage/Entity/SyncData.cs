using System;
using Tharga.Toolkit.LocalStorage.Repository;

namespace Tharga.Toolkit.LocalStorage.Entity
{
    class SyncData : IId
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public DateTime SyncTime { get; set; }
    }
}