using System;
using System.Collections.Generic;
using System.Linq;
using SampleBusiness.Interface;
using Tharga.Toolkit.LocalStorage.Business;

namespace SampleConsoleClient.Command
{
    public static class CustomerExtensions
    {
        public static Func<List<KeyValuePair<Guid, string>>> GetItemList(this RealmBusinessBase<ICustomerEntity> business)
        {
            return () =>
            {
                var list = business.GetAllAsync().Result;
                return list.Select(x => new KeyValuePair<Guid, string>(x.Id, x.Id.ToString())).ToList();
            };
        }
    }
}