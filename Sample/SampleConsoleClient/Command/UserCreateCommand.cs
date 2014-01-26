using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SampleBusiness.Business;
using SampleBusiness.Entities;
using SampleDataTransfer;
using Tharga.Toolkit.Console.Command.Base;

namespace SampleConsoleClient.Command
{
    internal class UserCreateCommand : ActionCommandBase
    {
        private readonly UserBusiness _userBusiness;
        private readonly RealmBusiness _realmBusiness;

        public UserCreateCommand(UserBusiness userBusiness, RealmBusiness realmBusiness)
            : base("create", "create a new user")
        {
            _userBusiness = userBusiness;
            _realmBusiness = realmBusiness;
        }

        public async override Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var userName = QueryParam<string>("UserName", GetParam(paramList, index++));
            var password = QueryParam<string>("Password", GetParam(paramList, index++));
            var realmId = QueryParam<Guid>("Realm", GetParam(paramList, index++), GetRealmList());

            await _userBusiness.SaveAsync(new UserEntity {UserName = userName, PasswordHash = Tools.GetHash(password), RealmId = realmId});

            return true;
        }

        private Func<List<KeyValuePair<Guid, string>>> GetRealmList()
        {
            return () =>
                {
                    var list = _realmBusiness.GetAllAsync().Result;
                    return list.Select(x => new KeyValuePair<Guid, string>(x.Id, x.Name.ToString(CultureInfo.InvariantCulture))).ToList();
                };
        }

    }
}