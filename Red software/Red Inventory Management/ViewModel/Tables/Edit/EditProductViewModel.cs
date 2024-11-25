using BusinessLayer;
using EntityLayer;
using Red_Inventory_Management.Model;
using System.Collections.Generic;

namespace Red_Inventory_Management.ViewModel
{
    class EditProductViewModel : EditItemModel<ProductListEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EditProductViewModel(ProductListEntity item, bool newRecord, string itemName) : base(item, newRecord, itemName) { }

        private List<string> _categoryList;
        public List<string> CategoryList
        {
            get
            {
                if (_categoryList == null)
                {
                    _categoryList = new List<string>();
                    var EntityList = ManageProducts.ListProductCategories();
                    foreach (var e in EntityList)
                    {
                        _categoryList.Add(e.Category);
                    }
                }
                return _categoryList;
            }
            set { SetProperty(ref _categoryList, value); }
        }

        protected override bool Save(object parameter)
        {
            log.Debug("Save " + ItemName);

            bool result = false;
            if (NewRecord)
            {
                result = ManageProducts.NewProduct(Item);
            }
            else
            {
                result = ManageProducts.ModifyProduct(Item);
            }
            return result;
        }
    }
}
