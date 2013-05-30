using SampleBusiness.Business;
using SampleBusiness.Interface;
using Tharga.Toolkit.StorageConsole.Command;

namespace SampleConsoleClient.Command
{
    public class RealmCommand : BusinessCommandBase<RealmBusiness, IRealmEntity>
    {
        public RealmCommand(RealmBusiness business) 
            : base("realm", business)
        {
            RegisterCommand(new RealmCreateCommand(business));
        }
    }
}