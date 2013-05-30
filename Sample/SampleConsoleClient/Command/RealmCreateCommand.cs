using System.Threading.Tasks;
using SampleBusiness.Business;
using SampleBusiness.Entities;
using Tharga.Toolkit.Console.Command.Base;

namespace SampleConsoleClient.Command
{
    internal class RealmCreateCommand : ActionCommandBase
    {
        private readonly RealmBusiness _realmBusiness;

        public RealmCreateCommand(RealmBusiness realmBusiness)
            : base("create", "create a new realm")
        {
            _realmBusiness = realmBusiness;
        }

        public async override Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var name = QueryParam<string>("Name", GetParam(paramList, index++));

            await _realmBusiness.SaveAsync(new RealmEntity {Name = name});

            return true;
        }        
    }
}