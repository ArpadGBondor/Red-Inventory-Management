using Red_Inventory_Management.ViewModel;
using System.Windows;

namespace Red_Inventory_Management.Views
{
    /// <summary>
    /// Interaction logic for NewUserWindow.xaml
    /// </summary>
    public partial class NewUserWindow : Window
    {
        public NewUserWindow()
        {
            InitializeComponent();
            NewUserViewModel NUVM = new NewUserViewModel();
            NUVM.NewUserWindow = this;
            this.DataContext = NUVM;
        }
    }
}
