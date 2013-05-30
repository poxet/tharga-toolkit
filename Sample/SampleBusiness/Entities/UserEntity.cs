using System;
using SampleBusiness.Converter;
using SampleBusiness.Interface;
using SampleDataTransfer.Entities;
using Tharga.Toolkit.LocalStorage.Entity;

namespace SampleBusiness.Entities
{
    public class UserEntity : EntityBaseWithValidation, IUserEntity
    {
        [Validator(Type = ValidatorAttribute.ValidatorType.MissingString)]
        public string UserName { get; set; }

        [Validator(Type = ValidatorAttribute.ValidatorType.MissingString)]
        public string PasswordHash { get; set; }

        [Validator(Type = ValidatorAttribute.ValidatorType.MissingGuid)]
        public Guid RealmId { get; set; }

        public static IUserEntity Convert(UserDto arg)
        {
            return new UserEntity
                {
                    Id = arg.Id,
                    UserName = arg.UserName,
                    PasswordHash = arg.PasswordHash,
                    RealmId = arg.RealmId,
                    StoreInfo = arg.StoreInfo.ToStoreInfo()
                };
        }
    }
}