using SampleBusiness.Interface;
using Tharga.Toolkit.LocalStorage.Business;

namespace SampleWpfClient.Model
{
    class CustomerModel : RealmModelBase<ICustomerEntity>
    {
        public CustomerModel(RealmBusinessBase<ICustomerEntity> business)
            : base(business, true)
        {

        }
    }
}