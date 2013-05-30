using SampleBusiness.Interface;
using Tharga.Toolkit.LocalStorage.Business;

namespace SampleWpfClient.Model
{
    class UserModel : ModelBase<IUserEntity>
    {
        public UserModel(BusinessBase<IUserEntity> business)
            : base(business, true)
        {

        }        
    }
}