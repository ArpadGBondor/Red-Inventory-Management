using EntityLayer;
using Red_software.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Red_software.ViewModel
{
    public class EditProductCategoryViewModel : EditItemModel<ProductCategoryEntity>
    {
        public EditProductCategoryViewModel(ProductCategoryEntity _Item, bool _NewRecord) : base(_Item, _NewRecord) { }
    }
}
