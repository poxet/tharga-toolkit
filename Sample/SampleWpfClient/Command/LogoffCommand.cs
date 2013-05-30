using System.Threading.Tasks;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient.Command
{
    public class LogoffCommand : CommandBase
    {
        public LogoffCommand(ISubscriptionHandler subscriptionHandler)
            : base(subscriptionHandler)
        {

        }

        public override bool CanExecute(object parameter)
        {
            var canExecute = _subscriptionHandler.IsOnline && !Executing && _subscriptionHandler.Session != null;
            //var canExecute = !Executing && _subscriptionHandler.Session != null;
            return canExecute;
        }

        protected override async Task DoExecute(object parameter)
        {
            await _subscriptionHandler.EndSession();
        }
    }
}