using System.Threading.Tasks;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient.Command
{
    public class ShowCreateSessionCommand : CommandBase
    {
        public ShowCreateSessionCommand(ISubscriptionHandler subscriptionHandler)
            : base(subscriptionHandler)
        {

        }

        public override bool CanExecute(object parameter)
        {
            return _subscriptionHandler.IsOnline && !Executing && _subscriptionHandler.Session == null;
        }

        protected override async Task DoExecute(object parameter)
        {
            var dlg = new View.Windows.CreateSession();
            dlg.ShowDialog();
        }
    }
}