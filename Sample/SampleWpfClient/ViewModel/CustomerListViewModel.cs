using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SampleBusiness.Interface;
using SampleWpfClient.Annotations;
using SampleWpfClient.Command;
using SampleWpfClient.Model;
using Tharga.Toolkit.LocalStorage.Business;

namespace SampleWpfClient.ViewModel
{
    public sealed class CustomerListViewModel : INotifyPropertyChanged
    {
        private readonly RealmBusinessBase<ICustomerEntity> _business;
        private readonly CustomerModel _customerModel;

        public IEnumerable<ICustomerEntity> Customers { get { return _customerModel.Items; } }

        public CustomerListViewModel()
        {
            _business = AggregateRoot.Instance.CustomerBusiness;

            _customerModel = new CustomerModel(_business);
            _customerModel.Items.CollectionChanged += Items_CollectionChanged;

            LoadCommand = new LoadCommand<ICustomerEntity>(_business, _customerModel);
            UnloadCommand = new UnloadCommand<ICustomerEntity>(_business, _customerModel);
            ResetCommand = new ResetCommand<ICustomerEntity>(_business, _customerModel);
            RefreshCommand = new RefreshCommand<ICustomerEntity>(_business, _customerModel);
            //DeleteCommand = new DeleteCommand<ICustomerEntity>(_business, _customerModel);
        }

        void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Customers");
        }

        //public async Task LoadAsync()
        //{
        //    var list = await _business.GetAllAsync();
        //    _customerModel.Items.Set(list);
        //}

        public ICommand LoadCommand { get; protected set; }
        public ICommand UnloadCommand { get; protected set; }
        public ICommand ResetCommand { get; protected set; }
        public ICommand RefreshCommand { get; protected set; }
        //public ICommand DeleteCommand { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}