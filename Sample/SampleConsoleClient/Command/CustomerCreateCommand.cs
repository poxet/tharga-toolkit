using System.Threading.Tasks;
using SampleBusiness.Entities;
using SampleBusiness.Interface;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;

namespace SampleConsoleClient.Command
{
    internal class CustomerCreateCommand : SessionActionCommandBase<ICustomerEntity>
    {
        private readonly RealmBusinessBase<ICustomerEntity> _business;

        public CustomerCreateCommand(RealmBusinessBase<ICustomerEntity> business)
            : base(business, "create", "create a new customer")
        {
            _business = business;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var name = QueryParam<string>("Name", GetParam(paramList, index++));
            var address = QueryParam<string>("Address", GetParam(paramList, index++));

            await _business.SaveAsync(new CustomerEntity { Name = name, Address = address});

            return true;
        }
    }
}