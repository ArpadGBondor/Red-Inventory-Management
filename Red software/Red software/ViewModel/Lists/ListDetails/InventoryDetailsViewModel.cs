using EntityLayer;
using BusinessLayer;
using Red_Inventory_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Red_Inventory_Management.ViewModel
{
    public class InventoryDetailsViewModel : ListModel<TransactionHeadListEntity>
    {
        private InventoryDetailsViewModel() { }
        public InventoryDetailsViewModel(int _Product_Id)
            :this()
        {
            Product_Id = _Product_Id;
            RefreshList(null);
        }
        private int Product_Id;

        private decimal totalQuantity;
        public decimal TotalQuantity
        {
            get { return totalQuantity; }
            set { SetProperty(ref totalQuantity, value); }
        }
        protected override void RefreshList(object parameter)
        {
            List = ManageTransactions.ListInventoryDetails(Product_Id);
            TotalQuantity = 0;
            foreach (var record in List)
                TotalQuantity += record.ListVariable;
        }
    }
}
