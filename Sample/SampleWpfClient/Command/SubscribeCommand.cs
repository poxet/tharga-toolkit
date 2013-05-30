using System.Threading.Tasks;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient.Command
{
    public class SubscribeCommand : CommandBase
    {
        public SubscribeCommand(ISubscriptionHandler subscriptionHandler) 
            : base(subscriptionHandler)
        {

        }

        public override bool CanExecute(object parameter)
        {
            return !_subscriptionHandler.IsOnline && !Executing;
        }

        protected override async Task DoExecute(object parameter)
        {
            await _subscriptionHandler.StartSubscriptionAsync();
        }
    }
}