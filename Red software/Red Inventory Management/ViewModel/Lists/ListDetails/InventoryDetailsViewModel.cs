using BusinessLayer;
using EntityLayer;
using Red_Inventory_Management.Model;

namespace Red_Inventory_Management.ViewModel
{
    public class InventoryDetailsViewModel : ListModel<TransactionHeadListEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private InventoryDetailsViewModel() { }
        public InventoryDetailsViewModel(int productId)
            : this()
        {
            _productId = productId;
            RefreshList(null);
        }
        private int _productId;

        private decimal _totalQuantity;
        public decimal TotalQuantity
        {
            get { return _totalQuantity; }
            set { SetProperty(ref _totalQuantity, value); }
        }
        protected override void RefreshList(object parameter)
        {
            log.Debug("Refresh list: Inventory list details");

            List = ManageTransactions.ListInventoryDetails(_productId);
            TotalQuantity = 0;
            foreach (var record in List)
                TotalQuantity += record.ListVariable;
        }
    }
}
