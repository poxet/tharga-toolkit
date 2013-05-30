using System;
using System.Linq;
using SampleConsoleHost.Business;
using SampleDataTransfer;
using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.Storage;

namespace SampleConsoleHost.Handlers
{
    public class CreateSessionHandler : ICreateSessionHandler<CreateSessionRequest>
    {
        public ISession Handle(CreateSessionRequest command)
        {
            var userBusiness = new UserBusiness();
            var user = userBusiness.GetAll(Guid.Empty).FirstOrDefault(x => string.Compare(x.UserName, command.UserName, StringComparison.InvariantCultureIgnoreCase) == 0 && string.Compare(x.PasswordHash, Tools.GetHash(command.Password), StringComparison.InvariantCulture) == 0);

            if (user == null)
                return new Session();

            return new Session
                {
                    SessionToken = Guid.NewGuid(),
                    RealmId = user.RealmId
                };
        }
    }
}