using System;
using SampleBusiness.Converter;
using SampleBusiness.Interface;
using SampleDataTransfer.Command;
using Tharga.Toolkit.LocalStorage.Utility;

namespace SampleBusiness.Repository.Service
{
    public class UserServiceRepository : ServiceRepository<IUserEntity>
    {
        public override void Save(Guid sessionToken, IUserEntity item, bool notifySubscribers)
        {
            var command = new SaveUserCommand { User = item.ToUserDto(), NotifySubscribers = notifySubscribers };
            WcfShell.Execute(CreateCommandClient, client => client.Execute(sessionToken, command));
        }

        public override void Delete(Guid sessionToken, Guid id)
        {
            WcfShell.Execute(CreateCommandClient, client => client.Execute(sessionToken, new DeleteUserCommand { Id = id }));
        }
    }
}