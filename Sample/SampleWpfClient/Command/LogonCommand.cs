using System.Threading.Tasks;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient.Command
{    
    public class LogonCommand : CommandBase
    {
        public LogonCommand(ISubscriptionHandler subscriptionHandler)
            : base(subscriptionHandler)
        {
            
        }

        public override bool CanExecute(object parameter)
        {
            return _subscriptionHandler.IsOnline && !Executing && _subscriptionHandler.Session == null;
        }

        protected override async Task DoExecute(object parameter)
        {
            var d = parameter as dynamic;
            await _subscriptionHandler.CreateSession(d.UserName, d.Password);
        }
    }
}