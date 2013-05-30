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
    public sealed class UserListViewModel : INotifyPropertyChanged
    {
        private readonly BusinessBase<IUserEntity> _business;
        private readonly UserModel _userModel;

        public IEnumerable<IUserEntity> Users { get { return _userModel.Items; } }

        public UserListViewModel()
        {
            _business = AggregateRoot.Instance.UserBusiness;

            _userModel = new UserModel(_business);
            _userModel.Items.CollectionChanged += Items_CollectionChanged;

            LoadCommand = new LoadCommand<IUserEntity>(_business, _userModel);
            UnloadCommand = new UnloadCommand<IUserEntity>(_business, _userModel);
            ResetCommand = new ResetCommand<IUserEntity>(_business, _userModel);
            RefreshCommand = new RefreshCommand<IUserEntity>(_business, _userModel);
            //LogonCommand = new LogonCommand<IUserEntity>(_business, _userModel);
            //DeleteCommand = new DeleteCommand<IUserEntity>(_business, _userModel);
        }

        void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Users");
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
        //public ICommand LogonCommand { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}