using System;
using SampleBusiness.Interface;
using SampleDataTransfer.Entities;
using Tharga.Toolkit.LocalStorage.Entity;
using Tharga.Toolkit.ServerStorage.DataTransfer;

namespace SampleBusiness.Converter
{
    public static class Converter
    {
        public static RealmDto ToRealmDto(this IRealmEntity item)
        {
            return new RealmDto
            {
                Id = item.Id,
                Name = item.Name,
                StoreInfo = item.StoreInfo.ToStoreInfoDto(),
            };
        }

        public static UserDto ToUserDto(this IUserEntity item)
        {
            return new UserDto
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    PasswordHash = item.PasswordHash,
                    RealmId = item.RealmId,
                    StoreInfo = item.StoreInfo.ToStoreInfoDto(),
                };
        }

        public static CustomerDto ToCustomerDto(this ICustomerEntity item)
        {
            return new CustomerDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Address = item.Address,
                    StoreInfo = item.StoreInfo.ToStoreInfoDto()
                };
        }

        public static ProductDto ToProductDto(this IProductEntity item)
        {
            return new ProductDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    StoreInfo = item.StoreInfo.ToStoreInfoDto()
                };
        }

        private static StoreInfoDto ToStoreInfoDto(this StoreInfo item)
        {
            return new StoreInfoDto
                {
                    LocalMachineName = item.LocalMachineName,
                    LocalStoreTime = item.LocalStoreTime,
                    LocalUserName = item.LocalUserName,
                    ServerStoreTime = item.ServerStoreTime
                };
        }

        public static StoreInfo ToStoreInfo(this StoreInfoDto dto)
        {
            if (dto.ServerStoreTime == null) throw new ArgumentException("dto.ServerStoreTime is null.");
            if (dto.LocalStoreTime == null) throw new ArgumentException("dto.LocalStoreTime is null.");

            return StoreInfo.Build(dto.ServerStoreTime.Value, dto.LocalStoreTime.Value, dto.LocalMachineName, dto.LocalUserName);
        }
    }
}
