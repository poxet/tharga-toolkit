using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.Storage;

namespace SampleConsoleHost.Handlers
{
    public class EndSessionHandler : IEndSessionHandler<EndSessionRequest>
    {
        public ISession Handle(EndSessionRequest command)
        {
            //NOTE: Optional action when the user is ending the session

            return new Session
                {
                    SessionToken = command.SessionToken
                };
        }
    }
}