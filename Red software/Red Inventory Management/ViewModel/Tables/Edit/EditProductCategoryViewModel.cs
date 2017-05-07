using System;
using EntityLayer;
using Red_Inventory_Management.Model;
using BusinessLayer;
using Red_Inventory_Management.Notifications;

namespace Red_Inventory_Management.ViewModel
{
    public class EditProductCategoryViewModel : EditItemModel<ProductCategoryEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EditProductCategoryViewModel(ProductCategoryEntity item, bool newRecord, string itemName) : base(item, newRecord, itemName) { }

        protected override bool Save(object parameter)
        {
            log.Debug("Save " + ItemName);

            bool result = false;
            if (string.IsNullOrWhiteSpace(Item.Category))
            {
                NotificationProvider.Error((NewRecord ? "New" : "Edit") + " product category error", "Please fill the category field.");
            }
            else
            {
                if (NewRecord)
                {
                    result = ManageProducts.NewProductCategory(Item);
                }
                else
                {
                    result = ManageProducts.ModifyProductCategory(Item);
                }
                if (!result)
                    NotificationProvider.Error((NewRecord ? "New" : "Edit") + " product category error", "Category name already exist.");
            }
            return result;
        }
    }
}
