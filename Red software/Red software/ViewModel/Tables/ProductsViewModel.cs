using Red_software.Model;
using Red_software.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EntityLayer;
using BusinessLayer;
using Red_software.Views;

namespace Red_software.ViewModel
{
    public class ProductsViewModel : TableModel<ProductListEntity>
    {
        protected override void DeleteItem(object parameter)
        {        
            if (ManageProducts.DeleteProduct(SelectedItem))
                RefreshList(parameter);
            else
            {
                NotificationProvider.Error("Delete product error", "This product is set to one or more transactions.");
            }
        }

        protected override void EditItem(object parameter)
        {
            ProductListEntity Item = new ProductListEntity();
            EntityCloner.CloneProperties<ProductListEntity>(Item, SelectedItem);
            EditProductViewModel EPVM = new EditProductViewModel(Item, false);
            EditItemView EIV = new EditItemView() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.Id == p.Id)
                        SelectedItem = p;
            }
        }

        protected override void NewItem(object parameter)
        {
            ProductListEntity Item = new ProductListEntity();
            EditProductViewModel EPVM = new EditProductViewModel(Item, true);
            EditItemView EIV = new EditItemView() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.Id == p.Id)
                        SelectedItem = p;
            }
        }

        protected override void RefreshList(object parameter)
        {
            List = ManageProducts.ListProducts();
        }
    }

}
