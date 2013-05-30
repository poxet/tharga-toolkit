using System;
using Tharga.Toolkit.ServerStorage.DataTransfer;

namespace Tharga.Toolkit.ServerStorage.Interface
{
    public interface IDto
    {
        Guid Id { get; set; }
        StoreInfoDto StoreInfo { get; set; }
    }
}
