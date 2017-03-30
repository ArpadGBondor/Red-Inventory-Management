using System;
using EntityLayer;
using Red_Inventory_Management.Model;
using BusinessLayer;
using Red_Inventory_Management.Notifications;

namespace Red_Inventory_Management.ViewModel
{
    public class EditProductCategoryViewModel : EditItemModel<ProductCategoryEntity>
    {
        public EditProductCategoryViewModel(ProductCategoryEntity _Item, bool _NewRecord, string _ItemName) : base(_Item, _NewRecord, _ItemName) { }

        protected override bool Save(object parameter)
        {
            bool lSuccess = false;
            if (string.IsNullOrWhiteSpace(Item.Category))
            {
                NotificationProvider.Error((NewRecord ? "New" : "Edit") + " product category error", "Please fill the category field.");
            }
            else
            {
                if (NewRecord)
                {
                    lSuccess = ManageProducts.NewProductCategory(Item);
                }
                else
                {
                    lSuccess = ManageProducts.ModifyProductCategory(Item);
                }
                if (!lSuccess)
                    NotificationProvider.Error((NewRecord ? "New" : "Edit") + " product category error", "Category name already exist.");
            }
            return lSuccess;
        }
    }
}
