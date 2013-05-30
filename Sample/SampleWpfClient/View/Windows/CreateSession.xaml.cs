using System.Windows;
using SampleWpfClient.Command;

namespace SampleWpfClient.View.Windows
{
    public partial class CreateSession : Window
    {
        public CreateSession()
        {
            InitializeComponent();

            AggregateRoot.Instance.SubscriptionHandler.SessionCreatedEvent += SubscriptionHandler_SessionCreatedEvent;
        }

        void SubscriptionHandler_SessionCreatedEvent(object sender, Tharga.Toolkit.LocalStorage.Entity.SessionCreatedEventArgs e)
        {
            if (!CheckAccess())
            {
                Dispatcher.Invoke(() => SubscriptionHandler_SessionCreatedEvent(sender, e));
                return;
            }

            Close();
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            var command = new LogonCommand(AggregateRoot.Instance.SubscriptionHandler);
            command.Execute(new { UserName = UserName.Text, Password = Password.Password });
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
