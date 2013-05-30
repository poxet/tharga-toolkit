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
    public sealed class RealmListViewModel: INotifyPropertyChanged
    {
        private readonly BusinessBase<IRealmEntity> _business;
        private readonly RealmModel _realmModel;

        public IEnumerable<IRealmEntity> Realms { get { return _realmModel.Items; } }

        public RealmListViewModel()
        {
            _business = AggregateRoot.Instance.RealmBusiness;

            _realmModel = new RealmModel(_business);
            _realmModel.Items.CollectionChanged += Items_CollectionChanged;

            LoadCommand = new LoadCommand<IRealmEntity>(_business, _realmModel);
            UnloadCommand = new UnloadCommand<IRealmEntity>(_business, _realmModel);
            ResetCommand = new ResetCommand<IRealmEntity>(_business, _realmModel);
            RefreshCommand = new RefreshCommand<IRealmEntity>(_business, _realmModel);
            //DeleteCommand = new DeleteCommand<IRealmEntity>(_business, _userModel);
        }

        void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Realm");
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