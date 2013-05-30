using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Tharga.Toolkit.LocalStorage.Entity;
using Tharga.Toolkit.LocalStorage.Interface;

namespace SampleWpfClient.Command
{
    public abstract class CommandBase : ICommand
    {
        protected readonly ISubscriptionHandler _subscriptionHandler;
        private readonly Dispatcher _dispatcher;
        private bool _executing;

        protected bool Executing
        {
            get { return _executing; }
            private set
            {
                if (_executing != value)
                {
                    _executing = value;
                    RefreshCanExecute();
                }
            }
        }

        public abstract bool CanExecute(object parameter);
        protected abstract Task DoExecute(object parameter);

        public async void Execute(object parameter)
        {
            try
            {
                Executing = true;
                
                await DoExecute(parameter);
            }
            finally
            {
                Executing = false;
            }
        }

        protected CommandBase(ISubscriptionHandler subscriptionHandler)
        {
            _subscriptionHandler = subscriptionHandler;
            _subscriptionHandler.SubscriptionStartedEvent += _subscriptionHandler_SubscriptionStartedEvent;
            _subscriptionHandler.SubscriptionStoppedEvent += _subscriptionHandler_SubscriptionStoppedEvent;
            _subscriptionHandler.SessionCreatedEvent += _subscriptionHandler_SessionCreatedEvent;
            _subscriptionHandler.SessionEndedEvent += _subscriptionHandler_SessionEndedEvent;

            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        private void _subscriptionHandler_SubscriptionStartedEvent(object sender, SubscriptionStartedEventArgs e)
        {
            RefreshCanExecute();
        }

        private void _subscriptionHandler_SubscriptionStoppedEvent(object sender, SubscriptionStoppedEventArgs e)
        {
            RefreshCanExecute();
        }
        
        private void _subscriptionHandler_SessionCreatedEvent(object sender, SessionCreatedEventArgs e)
        {
            RefreshCanExecute();
        }

        private void _subscriptionHandler_SessionEndedEvent(object sender, SessionEndedEventArgs e)
        {
            RefreshCanExecute();
        }

        protected void RefreshCanExecute()
        {
            if (Thread.CurrentThread != _dispatcher.Thread)
            {
                _dispatcher.BeginInvoke((Action)(RefreshCanExecute));
                return;
            }

            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

        public event EventHandler CanExecuteChanged;
    }
}