using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Red_Inventory_Management.Model;
using EntityLayer;
using BusinessLayer;
using Red_Inventory_Management.Views;
using Red_Inventory_Management.Notifications;

namespace Red_Inventory_Management.ViewModel
{
    public class ProductCategoriesViewModel : TableModel<ProductCategoryEntity>
    {
        public ProductCategoriesViewModel()
        {
            ItemName = "category";
            TableName = "Product categories";
        }

        protected override void DeleteItem(object parameter)
        {
            string name = SelectedItem.Category;
            if (ManageProducts.DeleteProductCategory(SelectedItem))
            {
                RefreshList(parameter);
                NotificationProvider.Info("Product category deleted", string.Format("Category name:{0}", name));
            }
            else
            {
                NotificationProvider.Error("Delete product category error", "This category is set to one or more product");
            }
        }

        protected override void EditItem(object parameter)
        {
            ProductCategoryEntity Item = new ProductCategoryEntity();
            EntityCloner.CloneProperties<ProductCategoryEntity>(Item, SelectedItem);
            EditProductCategoryViewModel EPVM = new EditProductCategoryViewModel(Item, false, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                Item = EPVM.Item;
                NotificationProvider.Info("Product category saved", string.Format("Category name:{0}", Item.Category));
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.Id == p.Id)
                        SelectedItem = p;
            }
        }

        protected override void NewItem(object parameter)
        {
            ProductCategoryEntity Item = new ProductCategoryEntity();
            EditProductCategoryViewModel EPVM = new EditProductCategoryViewModel(Item, true, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                Item = EPVM.Item;
                NotificationProvider.Info("Product category added", string.Format("Category name:{0}", Item.Category));
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.Id == p.Id)
                        SelectedItem = p;
            }
        }

        protected override void RefreshList(object parameter)
        {
            List = ManageProducts.ListProductCategories();
        }
    }
}
