using Red_Inventory_Management.Model;
using EntityLayer;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Red_Inventory_Management.Views;

namespace Red_Inventory_Management.ViewModel
{
    public class InventoryViewModel : ListModel<TransactionBodyListEntity>
    {
        public InventoryViewModel()
        {
            TableName = "Inventory list";
        }
        protected override void RefreshList(object parameter)
        {
            List = ManageTransactions.ListInventory();
        }

        private ICommand detailsCommand;
        public ICommand DetailsCommand
        {
            get
            {
                if (detailsCommand == null) detailsCommand = new RelayCommand(new Action<object>(Details), new Predicate<object>(DetailsCanExecute));
                return detailsCommand;
            }
            set { SetProperty(ref detailsCommand, value); }
        }

        private bool DetailsCanExecute(object parameter)
        {
            return ItemSelected(parameter);
        }

        private void Details(object parameter)
        {
            InventoryDetailsViewModel IDVM = new InventoryDetailsViewModel(SelectedItem.Product.Id);
            ListDetailsWindow LDW = new ListDetailsWindow() { DataContext = IDVM };
            LDW.ShowDialog();
        }
    }
}
