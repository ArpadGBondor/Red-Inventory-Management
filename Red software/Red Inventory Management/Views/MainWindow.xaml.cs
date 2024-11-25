using Red_Inventory_Management.Notifications;
using Red_Inventory_Management.ViewModel;
using System.Windows;

namespace Red_Inventory_Management.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(this);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NotificationProvider.Close();
        }
    }
}
