using System;
using SampleBusiness.Converter;
using SampleBusiness.Interface;
using SampleDataTransfer.Command;
using Tharga.Toolkit.LocalStorage.Utility;

namespace SampleBusiness.Repository.Service
{
    public class RealmServiceRepository : ServiceRepository<IRealmEntity>
    {
        public override void Save(Guid sessionToken, IRealmEntity item, bool notifySubscribers)
        {
            var command = new SaveRealmCommand { Realm = item.ToRealmDto(), NotifySubscribers = notifySubscribers };
            WcfShell.Execute(CreateCommandClient, client => client.Execute(sessionToken, command));
        }

        public override void Delete(Guid sessionToken, Guid id)
        {
            WcfShell.Execute(CreateCommandClient, client => client.Execute(sessionToken, new DeleteRealmCommand { Id = id }));
        }
    }
}