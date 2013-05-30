using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SampleWpfClient.Annotations;
using SampleWpfClient.Command;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient.ViewModel
{
    public sealed class CreateSessionViewModel : INotifyPropertyChanged
    {
        private readonly ISubscriptionHandler _subscriptionHandler;

        public CreateSessionViewModel()
        {
            _subscriptionHandler = AggregateRoot.Instance.SubscriptionHandler;
            LogonCommand = new LogonCommand(_subscriptionHandler);
        }

        public ICommand LogonCommand { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}