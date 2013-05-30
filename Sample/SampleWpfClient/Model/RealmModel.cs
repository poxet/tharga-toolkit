using SampleBusiness.Interface;
using Tharga.Toolkit.LocalStorage.Business;

namespace SampleWpfClient.Model
{
    class RealmModel : ModelBase<IRealmEntity>
    {
        public RealmModel(BusinessBase<IRealmEntity> business)
            : base(business, true)
        {

        }                
    }
}