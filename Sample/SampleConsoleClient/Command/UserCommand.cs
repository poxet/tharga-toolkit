using SampleBusiness.Business;
using SampleBusiness.Interface;
using Tharga.Toolkit.StorageConsole.Command;

namespace SampleConsoleClient.Command
{
    public class UserCommand : BusinessCommandBase<UserBusiness, IUserEntity>
    {
        public UserCommand(UserBusiness business, RealmBusiness realmBusiness)
            : base("user", business)
        {
            SetEntityOutput(OutputAction);

            RegisterCommand(new UserLogonCommand(_console, business.SubscriptionHandler));
            RegisterCommand(new UserLogoffCommand(_console, business.SubscriptionHandler));
            RegisterCommand(new UserCreateCommand(business, realmBusiness));
        }

        private string OutputAction(IUserEntity arg)
        {
            return string.Format("{0}\t{1}", arg.Id, arg.UserName);
        }
    }
}