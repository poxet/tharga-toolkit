using SampleBusiness.Converter;
using SampleBusiness.Interface;
using SampleDataTransfer.Entities;
using Tharga.Toolkit.LocalStorage.Entity;

namespace SampleBusiness.Entities
{
    public class RealmEntity : EntityBase, IRealmEntity
    {
        public string Name { get; set; }

        public static IRealmEntity Convert(RealmDto arg)
        {
            return new RealmEntity
                {
                    Id = arg.Id,
                    Name = arg.Name,
                    StoreInfo = arg.StoreInfo.ToStoreInfo()
                };
        }
    }
}