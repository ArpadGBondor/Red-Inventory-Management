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
    public class ProductsViewModel : TableModel<ProductEntity>
    {
        protected override void DeleteItem(object parameter)
        {
            NotificationProvider.Info("Delete record", ManageProducts.DeleteProduct(SelectedItem) ? "success" : "failed");
            RefreshList(parameter);
        }

        protected override void EditItem(object parameter)
        {
            ProductEntity product = new ProductEntity();
            EntityCloner.CloneProperties<ProductEntity>(product, SelectedItem);
            EditProductViewModel EPVM = new EditProductViewModel(product, false);
            EditItemView EIV = new EditItemView() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                product = EPVM.Item;
                NotificationProvider.Info("Edit record", ManageProducts.ModifyProduct(product) ? "success" : "failed");
                RefreshList(parameter);
                foreach (var p in List)
                    if (product.Id == p.Id)
                        SelectedItem = p;
            }
        }

        protected override void NewItem(object parameter)
        {
            ProductEntity product = new ProductEntity();
            EditProductViewModel EPVM = new EditProductViewModel(product, true);
            EditItemView EIV = new EditItemView() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                NotificationProvider.Info("New record", ManageProducts.NewProduct(EPVM.Item) ? "success" : "failed");
                RefreshList(parameter);
                foreach (var p in List)
                    if (product.Id == p.Id)
                        SelectedItem = p;
            }
        }

        protected override void RefreshList(object parameter)
        {
            this.List = ManageProducts.ListProducts();
        }
    }

}
