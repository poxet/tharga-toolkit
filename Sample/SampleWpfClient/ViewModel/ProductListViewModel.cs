using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SampleBusiness.Business;
using SampleBusiness.Interface;
using SampleWpfClient.Annotations;
using SampleWpfClient.Command;
using SampleWpfClient.Model;

namespace SampleWpfClient.ViewModel
{
    public sealed class ProductListViewModel : INotifyPropertyChanged
    {
        private readonly ProductBusiness _business;
        private readonly ProductModel _productModel;

        public IEnumerable<IProductEntity> Products { get { return _productModel.Items; } }

        public ProductListViewModel()
        {
            _business = AggregateRoot.Instance.ProductBusiness;

            _productModel = new ProductModel(_business);
            _productModel.Items.CollectionChanged += Items_CollectionChanged;

            LoadCommand = new LoadCommand<IProductEntity>(_business, _productModel);
            UnloadCommand = new UnloadCommand<IProductEntity>(_business, _productModel);
            ResetCommand = new ResetCommand<IProductEntity>(_business, _productModel);
            RefreshCommand = new RefreshCommand<IProductEntity>(_business, _productModel);
            //DeleteCommand = new DeleteCommand<IProductEntity>(_business, _productModel);
        }

        void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Products");
        }

        //public async Task LoadAsync()
        //{
        //    var list = await _business.GetAllAsync();
        //    _productModel.Items.Set(list);
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
