using System.Threading.Tasks;
using SampleBusiness.Business;
using SampleBusiness.Entities;
using Tharga.Toolkit.Console.Command.Base;

namespace SampleConsoleClient.Command
{
    internal class ProductCreateCommand : ActionCommandBase
    {
        private readonly ProductBusiness _business;

        public ProductCreateCommand(ProductBusiness business) 
            : base("create", "create a new product")
        {
            _business = business;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var name = QueryParam<string>("Name", GetParam(paramList, index++));

            var productEntity = new ProductEntity {Name = name};
            AssignVariables(productEntity, paramList);

            await _business.SaveAsync(productEntity);

            return true;
        }
    }
}