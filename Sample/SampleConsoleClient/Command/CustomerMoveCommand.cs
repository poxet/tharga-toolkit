//using System;
//using System.Threading.Tasks;
//using SampleBusiness.Business;
//using SampleBusiness.Interface;
//using Tharga.Toolkit.Console.Command.Base;
//using Tharga.Toolkit.LocalStorage.Business;

//namespace SampleConsoleClient.Command
//{
//    internal class CustomerMoveCommand : SessionActionCommandBase<ICustomerEntity>
//    {
//        private readonly RealmBusinessBase<ICustomerEntity> _business;

//        public CustomerMoveCommand(RealmBusinessBase<ICustomerEntity> business)
//            : base(business, "move", "Moves the customer to a new address")
//        {
//            _business = business;
//        }

//        public override async Task<bool> InvokeAsync(string paramList)
//        {
//            var index = 0;
//            var id = QueryParam<Guid>("Id", GetParam(paramList, index++), _business.GetItemList());
//            var item = await _business.GetAsync(id);
//            var newAddress = QueryParam<string>("New Address", GetParam(paramList, index++), item.Name);

//            await _business.MoveAsync(id, newAddress);

//            return true;
//        }
//    }
//}