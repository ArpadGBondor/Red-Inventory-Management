using System;
using EntityLayer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Red_Inventory_Management.Model
{
    public class BindableTransactionBodyListEntity : BindableBase
    {
        private BindableTransactionBodyListEntity() { }
        public BindableTransactionBodyListEntity(TransactionBodyListEntity _Item)
        {
            Item = _Item;
        }

        private TransactionBodyListEntity item;
        public TransactionBodyListEntity Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        public string ProductName { get { return Item.Product.Name; } }
        public string ProductCode { get { return Item.Product.Code; } }
        public decimal SumPrice
        {
            get
            {
                if (Item.Body == null) return 0;
                return Item.Body.Price * Item.Body.Quantity;
            }
        }

        public decimal Price
        {
            get { return Item.Body.Price; }
            set
            {
                if (Equals(Item.Body.Price, value)) return;
                Item.Body.Price = value;
                RaisePropertyChanged("Price");
                RaisePropertyChanged("SumPrice");
            }
        }

        public decimal Quantity
        {
            get { return Item.Body.Quantity; }
            set
            {
                if (Equals(Item.Body.Quantity, value)) return;
                Item.Body.Quantity = value;
                RaisePropertyChanged("Quantity");
                RaisePropertyChanged("SumPrice");
            }
        }

    }
}
