using System;
using System.Threading.Tasks;
using SampleBusiness.Business;
using SampleBusiness.Interface;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.LocalStorage.Business;

namespace SampleConsoleClient.Command
{
    //TODO: Fix this class
    //internal class CustomerRenameCommand : SessionActionCommandBase<ICustomerEntity>
    //{
    //    private readonly RealmBusinessBase<ICustomerEntity> _business;

    //    public CustomerRenameCommand(RealmBusinessBase<ICustomerEntity> business)
    //        : base(business, "rename", "rename a customer")
    //    {
    //        _business = business;
    //    }

    //    public override async Task<bool> InvokeAsync(string paramList)
    //    {
    //        var index = 0;
    //        var id = QueryParam<Guid>("Id", GetParam(paramList, index++), _business.GetItemList());
    //        var item = await _business.GetAsync(id);
    //        var name = QueryParam<string>("Name", GetParam(paramList, index++), item.Name);

    //        item.Name = name;

    //        await _business.SaveAsync(item);

    //        return true;
    //    }
    //}
}