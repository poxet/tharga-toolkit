using SampleDataTransfer.Entities;
using Tharga.Toolkit.ServerStorage;

namespace SampleConsoleHost.Business
{
    class RealmBusiness : BusinessBase<RealmDto, RealmDto>
    {
        protected override RealmDto Convert(RealmDto input)
        {
            return input;
        }
    }
}