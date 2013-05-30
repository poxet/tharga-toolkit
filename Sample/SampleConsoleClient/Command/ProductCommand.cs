using SampleBusiness.Business;
using SampleBusiness.Interface;
using Tharga.Toolkit.StorageConsole.Command;

namespace SampleConsoleClient.Command
{
    internal class ProductCommand : BusinessCommandBase<ProductBusiness, IProductEntity>
    {
        public ProductCommand(ProductBusiness business)
            : base("product", business)
        {
            SetEntityOutput(OutputAction);

            RegisterCommand(new ProductCreateCommand(business));
            RegisterCommand(new ProductRenameCommand(business));
            //RegisterCommand(new MemberImportCommand());
            //RegisterCommand(new MemberExportCommand());
            //RegisterCommand(new MemberActivityCommand());
        }

        private string OutputAction(IProductEntity arg)
        {
            return string.Format("{0}\t{1}", arg.Id, arg.Name);
        }
    }
}