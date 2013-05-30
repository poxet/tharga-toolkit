using System.Windows;
using System.Windows.Controls;

namespace SampleWpfClient.View.Control
{
    public partial class ProductList : UserControl
    {
        public ProductList()
        {
            InitializeComponent();
        }

        private async void ProductList_OnLoaded(object sender, RoutedEventArgs e)
        {
            //await ((ProductListViewModel)DataContext).LoadAsync();
        }
    }
}
