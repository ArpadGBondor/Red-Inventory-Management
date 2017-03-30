using Red_Inventory_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Red_Inventory_Management.Views
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : UserControl
    {
        public ListView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ((RelayCommand)((ListModelParent)DataContext).RefreshListCommand).CheckAndExecute(sender);
        }
    }
}
