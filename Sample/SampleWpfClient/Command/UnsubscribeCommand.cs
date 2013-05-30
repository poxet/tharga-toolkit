using System.Threading.Tasks;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient.Command
{
    public class UnsubscribeCommand : CommandBase
    {
        public UnsubscribeCommand(ISubscriptionHandler subscriptionHandler)
            :base(subscriptionHandler)
        {

        }

        public override bool CanExecute(object parameter)
        {
            return _subscriptionHandler.IsOnline && !Executing;
        }

        protected override async Task DoExecute(object parameter)
        {
            if (_subscriptionHandler.Session != null)
                await _subscriptionHandler.EndSession();
            await _subscriptionHandler.StopSubscriptionAsync();
        }
    }
}