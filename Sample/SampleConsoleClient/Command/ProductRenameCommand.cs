using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleBusiness.Business;
using Tharga.Toolkit.Console.Command.Base;

namespace SampleConsoleClient.Command
{
    internal class ProductRenameCommand : ActionCommandBase
    {
        private readonly ProductBusiness _business;

        public ProductRenameCommand(ProductBusiness business)
            : base("rename", "rename a product")
        {
            _business = business;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var id = QueryParam<Guid>("Id", GetParam(paramList, index++), GetItemList());
            var item = await _business.GetAsync(id);
            var name = QueryParam<string>("Name", GetParam(paramList, index++), item.Name);

            item.Name = name;

            await _business.SaveAsync(item);

            return true;
        }

        protected Func<List<KeyValuePair<Guid, string>>> GetItemList()
        {
            return () =>
                {
                    var list = _business.GetAllAsync().Result;
                    return list.Select(x => new KeyValuePair<Guid, string>(x.Id, x.Id.ToString())).ToList();
                };
        }
    }
}