using SampleDataTransfer.Entities;
using Tharga.Toolkit.ServerStorage;

namespace SampleConsoleHost.Business
{
    class UserBusiness : BusinessBase<UserDto, UserDto>
    {
        protected override UserDto Convert(UserDto input)
        {
            return input;
        }
    }
}