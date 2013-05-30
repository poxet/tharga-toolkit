using System.Windows;
using System.Windows.Controls;

namespace SampleWpfClient.View.Control
{
    public partial class CustomerList : UserControl
    {
        public CustomerList()
        {
            InitializeComponent();
        }

        private async void CustomerList_OnLoaded(object sender, RoutedEventArgs e)
        {
            //await ((CustomerListViewModel)DataContext).LoadAsync();
        }
    }
}
