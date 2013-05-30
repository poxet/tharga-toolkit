using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using SampleWpfClient.Annotations;
using SampleWpfClient.Command;
using Tharga.Toolkit;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient.ViewModel
{
    public sealed class MessageListViewModel : INotifyPropertyChanged
    {
        private readonly ISubscriptionHandler _subscriptionHandler;
        public SafeObservableCollection<MessageViewModel> Messages { get; private set; }

        public MessageListViewModel()
        {
            _subscriptionHandler = AggregateRoot.Instance.SubscriptionHandler;
            Messages = new SafeObservableCollection<MessageViewModel>();
            _subscriptionHandler.CheckOnlineStatusEvent += SubscriptionHandler_CheckOnlineStatusEvent;
            _subscriptionHandler.SubscriptionStartedEvent += SubscriptionHandler_SubscriptionStartedEvent;
            _subscriptionHandler.SubscriptionCheckedEvent += SubscriptionHandler_SubscriptionCheckedEvent;
            _subscriptionHandler.SubscriptionStoppedEvent += SubscriptionHandler_SubscriptionStoppedEvent;
            _subscriptionHandler.SubscriberChangeEvent += SubscriptionHandlerSubscriberChangeEvent;
            _subscriptionHandler.SessionCreatedEvent += _subscriptionHandler_SessionCreatedEvent;
            _subscriptionHandler.SessionEndedEvent += _subscriptionHandler_SessionEndedEvent;
            _subscriptionHandler.SessionCreatedFailedEvent += _subscriptionHandler_SessionCreatedFailedEvent;

            SubscribeCommand = new SubscribeCommand(_subscriptionHandler);
            UnsubscribeCommand = new UnsubscribeCommand(_subscriptionHandler);
            //LogonCommand = new LogonCommand(_subscriptionHandler);
            LogoffCommand = new LogoffCommand(_subscriptionHandler);
            //SubscribeCommand.CanExecuteChanged += SubscribeCommand_CanExecuteChanged;
            ShowCreateSessionCommand = new ShowCreateSessionCommand(_subscriptionHandler);
        }

        public ICommand SubscribeCommand { get; protected set; }
        public ICommand UnsubscribeCommand { get; protected set; }
        //public ICommand LogonCommand { get; protected set; }
        public ICommand LogoffCommand { get; protected set; }
        public ICommand ShowCreateSessionCommand { get; protected set; }

        void SubscriptionHandlerSubscriberChangeEvent(object sender, Tharga.Toolkit.LocalStorage.Entity.SubscriberChangeEventArgs e)
        {
            Messages.Add(new MessageViewModel("SubscriberChange"));
        }

        void _subscriptionHandler_SessionCreatedEvent(object sender, Tharga.Toolkit.LocalStorage.Entity.SessionCreatedEventArgs e)
        {
            Messages.Add(new MessageViewModel("SessionCreatedEvent"));
        }

        void _subscriptionHandler_SessionCreatedFailedEvent(object sender, Tharga.Toolkit.LocalStorage.Entity.SessionCreatedFailedEventArgs e)
        {
            Messages.Add(new MessageViewModel("SessionCreatedFailedEvent"));
        }

        void _subscriptionHandler_SessionEndedEvent(object sender, Tharga.Toolkit.LocalStorage.Entity.SessionEndedEventArgs e)
        {
            Messages.Add(new MessageViewModel("SessionEndedEvent"));            
        }

        void SubscriptionHandler_SubscriptionStoppedEvent(object sender, Tharga.Toolkit.LocalStorage.Entity.SubscriptionStoppedEventArgs e)
        {
            Messages.Add(new MessageViewModel("SubscriptionStopped"));
        }

        void SubscriptionHandler_SubscriptionCheckedEvent(object sender, Tharga.Toolkit.LocalStorage.Entity.SubscriptionCheckedEventArgs e)
        {
            Messages.Add(new MessageViewModel("SubscriptionChecked"));
        }

        void SubscriptionHandler_SubscriptionStartedEvent(object sender, Tharga.Toolkit.LocalStorage.Entity.SubscriptionStartedEventArgs e)
        {
            Messages.Add(new MessageViewModel("SubscriptionStarted"));
        }

        void SubscriptionHandler_CheckOnlineStatusEvent(object sender, Tharga.Toolkit.LocalStorage.Entity.CheckOnlineStatusEventArgs e)
        {
            Messages.Add(new MessageViewModel("CheckOnlineStatus"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task StartAsync()
        {
            await AggregateRoot.Instance.SubscriptionHandler.StartSubscriptionAsync();
        }
    }
}