using EntityLayer;
using Red_Inventory_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;

namespace Red_Inventory_Management.ViewModel
{
    class EditProductViewModel : EditItemModel<ProductListEntity>
    {
        public EditProductViewModel(ProductListEntity _Item, bool _NewRecord, string _ItemName) : base(_Item, _NewRecord, _ItemName) { }

        private List<string> categoryList;
        public List<string> CategoryList
        {
            get
            {
                if (categoryList == null)
                {
                    categoryList = new List<string>();
                    var EntityList = ManageProducts.ListProductCategories();
                    foreach (var e in EntityList)
                    {
                        categoryList.Add(e.Category);
                    }
                }
                return categoryList;
            }
            set { SetProperty(ref categoryList, value); }
        }

        protected override bool Save(object parameter)
        {
            bool lSuccess = false;
            if (NewRecord)
            {
                lSuccess = ManageProducts.NewProduct(Item);
            }
            else
            {
                lSuccess = ManageProducts.ModifyProduct(Item);
            }
            return lSuccess;
        }
    }
}
