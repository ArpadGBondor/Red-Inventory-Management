using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using Red_Inventory_Management.Model;
using BusinessLayer;
using Red_Inventory_Management.Notifications;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Red_Inventory_Management.ViewModel
{
    public class EditTransactionViewModel : EditItemModel<TransactionHeadListEntity>
    {
        // Constructor
        public EditTransactionViewModel(TransactionHeadListEntity _Item, bool _NewRecord, string _ItemName) : base(_Item, _NewRecord, _ItemName)
        {
            TransactionDate = Item.Date;

            this.TransactionBody.CollectionChanged += this.OnCollectionChanged;
            if (Item.Head.Id > 0)
            {
                var list = ManageTransactions.ListBody(Item.Head.Id);
                foreach (var record in list)
                    TransactionBody.Add(new BindableTransactionBodyListEntity(record));
            }

            if (Item.Head.Incoming)
                Partners = ManagePartners.ListDealers();
            else
                Partners = ManagePartners.ListCustomers();
            if (Item.Partner != null)
                foreach(var record in Partners.Where(p=>p.Id == Item.Partner.Id))
                    SelectedPartner = record;

            SelectedProductCategory = new ProductCategoryEntity() { Category = " - All product categories - ", Id = 0 };
            ProductCategories.Add(SelectedProductCategory);

            ProductCategories.AddRange(ManageProducts.ListProductCategories());
        }
        // Transaction date
        private DateTime transactionDate;
        public DateTime TransactionDate
        {
            get { return transactionDate; }
            set { SetProperty(ref transactionDate, value); }
        }

        // Partners
        private List<PartnerEntity> partners;
        public List<PartnerEntity> Partners
        {
            get { return partners; }
            set { SetProperty(ref partners, value); }
        }

        private PartnerEntity selectedPartner;
        public PartnerEntity SelectedPartner
        {
            get { return selectedPartner; }
            set { SetProperty(ref selectedPartner, value); }
        }

        // Products
        private List<ProductCategoryEntity> productCategories;
        public List<ProductCategoryEntity> ProductCategories
        {
            get
            {
                if (productCategories == null) productCategories = new List<ProductCategoryEntity>();
                return productCategories;
            }
            set { SetProperty(ref productCategories, value); }
        }

        private ProductCategoryEntity selectedProductCategory;
        public ProductCategoryEntity SelectedProductCategory
        {
            get { return selectedProductCategory; }
            set
            {
                if (selectedProductCategory != value) Products = ManageProducts.ListProducts(value.Id);
                SetProperty(ref selectedProductCategory, value);
            }
        }

        private List<ProductListEntity> products;
        public List<ProductListEntity> Products
        {
            get { return products; }
            set { SetProperty(ref products, value); }
        }

        private ProductListEntity selectedProduct;
        public ProductListEntity SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                if (value != null) ProductPrice = (Item.Head.Incoming ? value.Cost_Price : value.Sell_Price);
                SetProperty(ref selectedProduct, value);
            }
        }

        private decimal productQuantity;
        public decimal ProductQuantity
        {
            get { return productQuantity; }
            set { SetProperty(ref productQuantity, value); }
        }

        private decimal productPrice;
        public decimal ProductPrice
        {
            get { return productPrice; }
            set { SetProperty(ref productPrice, value); }
        }

        // Transaction body
        private ObservableCollection<BindableTransactionBodyListEntity> transactionBody;
        public ObservableCollection<BindableTransactionBodyListEntity> TransactionBody
        { 
            get
            {
                if (transactionBody == null) transactionBody = new ObservableCollection<BindableTransactionBodyListEntity>();
                return transactionBody;
            }
            set { SetProperty(ref transactionBody, value); }
        }

        private BindableTransactionBodyListEntity selectedBody;
        public BindableTransactionBodyListEntity SelectedBody
        {
            get { return selectedBody; }
            set { SetProperty(ref selectedBody, value); }
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            decimal sum = 0m;
            foreach (var record in TransactionBody)
                sum += record.SumPrice;
            TotalPrice = sum;
        }

        private decimal totalPrice;
        public decimal TotalPrice
        {
            get { return totalPrice; }
            set { SetProperty(ref totalPrice, value); }
        }

        private ICommand addProductCommand;
        public ICommand AddProductCommand
        {
            get
            {
                if (addProductCommand == null) addProductCommand = new RelayCommand(new Action<object>(AddProduct), new Predicate<object>(CanAddProduct));
                return addProductCommand;
            }
            set { SetProperty(ref addProductCommand, value); }
        }
        private void AddProduct(object parameter)
        {
            var rec = new TransactionBodyListEntity();
            rec.Product = new ProductEntity();
            rec.Product.Id = SelectedProduct.Id;
            rec.Product.Name = SelectedProduct.Name;
            rec.Product.Code = SelectedProduct.Code;
            rec.Product.Cost_Price = SelectedProduct.Cost_Price;
            rec.Product.Sell_Price = SelectedProduct.Sell_Price;
            rec.Body = new TransactionBodyEntity();
            rec.Body.Product_Id = SelectedProduct.Id;
            rec.Body.Price = ProductPrice;
            rec.Body.Quantity = ProductQuantity;
            TransactionBody.Add(new BindableTransactionBodyListEntity(rec));
            //NotificationProvider.Info("Add", "Product");
            //RaisePropertyChanged("TransactionBody");
        }
        private bool CanAddProduct(object parameter)
        {
            return (SelectedProduct != null);
        }

        private ICommand removeProductCommand;
        public ICommand RemoveProductCommand
        {
            get
            {
                if (removeProductCommand == null) removeProductCommand = new RelayCommand(new Action<object>(RemoveProduct), new Predicate<object>(CanRemoveProduct));
                return removeProductCommand;
            }
            set { SetProperty(ref removeProductCommand, value); }
        }
        private void RemoveProduct(object parameter)
        {
            TransactionBody.Remove(SelectedBody);
        }
        private bool CanRemoveProduct(object parameter)
        {
            return (SelectedBody != null);
        }

        protected override bool Save(object parameter)
        {
            if (SelectedPartner == null)
            {
                NotificationProvider.Error("Save transaction error", "Pleace select a partner.");
                return false;
            }
            else
            {
                Item.Head.Partner_Id = SelectedPartner.Id;
                Item.Head.TotalPrice = TotalPrice;
                Item.Date = TransactionDate;
                var list = new List<TransactionBodyListEntity>();
                foreach (var record in TransactionBody)
                    list.Add(record.Item);
                return ManageTransactions.AddOrModifyTransaction(Item.Head, list);
            }
        }
    }
}
