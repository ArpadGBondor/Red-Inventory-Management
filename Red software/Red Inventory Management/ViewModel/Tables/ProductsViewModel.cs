using Red_Inventory_Management.Model;
using Red_Inventory_Management.Notifications;
using EntityLayer;
using BusinessLayer;
using Red_Inventory_Management.Views;

namespace Red_Inventory_Management.ViewModel
{
    public class ProductsViewModel : TableModel<ProductListEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ProductsViewModel()
        {
            ItemName = "product";
            TableName = "Products";
        }
        protected override void DeleteItem(object parameter)
        {
            log.Debug("Delete " + ItemName + " button");

            string name = SelectedItem.Name;
            if (ManageProducts.DeleteProduct(SelectedItem))
            {
                RefreshList(parameter);
                NotificationProvider.Info("Product deleted", string.Format("Product name:{0}", name));
            }
            else
            {
                NotificationProvider.Error("Delete product error", "This product is set to one or more transactions.");
            }
        }

        protected override void EditItem(object parameter)
        {
            log.Debug("Edit " + ItemName + " button");

            ProductListEntity Item = new ProductListEntity();
            EntityCloner.CloneProperties<ProductListEntity>(SelectedItem, Item);
            EditProductViewModel EPVM = new EditProductViewModel(Item, false, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                Item = EPVM.Item;
                NotificationProvider.Info("Product saved", string.Format("Product name:{0}", Item.Name));
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.Id == p.Id)
                        SelectedItem = p;
            }
        }

        protected override void NewItem(object parameter)
        {
            log.Debug("New " + ItemName + " button");

            ProductListEntity Item = new ProductListEntity();
            EditProductViewModel EPVM = new EditProductViewModel(Item, true, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                Item = EPVM.Item;
                NotificationProvider.Info("Product added", string.Format("Product name:{0}", Item.Name));
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.Id == p.Id)
                        SelectedItem = p;
            }
        }

        protected override void RefreshList(object parameter)
        {
            log.Debug("Refresh " + ItemName + " list");

            List = ManageProducts.ListProducts();
        }
    }

}
