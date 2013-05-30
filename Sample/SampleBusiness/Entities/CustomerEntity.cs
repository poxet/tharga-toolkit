using SampleBusiness.Converter;
using SampleBusiness.Interface;
using SampleDataTransfer.Entities;
using Tharga.Toolkit.LocalStorage.Entity;

namespace SampleBusiness.Entities
{
    public class CustomerEntity : EntityBaseWithValidation, ICustomerEntity
    {
        [Validator(Type = ValidatorAttribute.ValidatorType.MissingString)]
        public string Name { get; set; }

        [Validator(Type = ValidatorAttribute.ValidatorType.MissingString)]
        public string Address { get; set; }

        internal static ICustomerEntity Convert(CustomerDto dto)
        {
            return new CustomerEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                Address = dto.Address,
                StoreInfo = dto.StoreInfo.ToStoreInfo()
            };
        }
    }
}