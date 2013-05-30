using System.Windows;
using System.Windows.Controls;

namespace SampleWpfClient.View.Control
{
    public partial class MessageList : UserControl
    {
        public MessageList()
        {
            InitializeComponent();
        }

        private async void MessageList_OnLoaded(object sender, RoutedEventArgs e)
        {
            //await ((MessageListViewModel)DataContext).StartAsync();
        }
    }
}
