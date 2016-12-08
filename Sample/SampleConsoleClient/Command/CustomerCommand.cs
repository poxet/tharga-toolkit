using SampleBusiness.Interface;
using Tharga.Toolkit.LocalStorage.Business;
using Tharga.Toolkit.StorageConsole.Command;

namespace SampleConsoleClient.Command
{
    internal class CustomerCommand : SessionBusinessCommandBase<RealmBusinessBase<ICustomerEntity>, ICustomerEntity>
    {
        public CustomerCommand(RealmBusinessBase<ICustomerEntity> business)
            : base("customer", business)
        {
            SetEntityOutput(OutputAction);

            //RegisterCommand(new CustomerCreateCommand(business));
            //RegisterCommand(new CustomerRenameCommand(business));
            ////RegisterCommand(new CustomerMoveCommand(business));
        }

        private string OutputAction(ICustomerEntity arg)
        {
            return string.Format("{0}\t{1}", arg.Id, arg.Name);
        }
    }
}