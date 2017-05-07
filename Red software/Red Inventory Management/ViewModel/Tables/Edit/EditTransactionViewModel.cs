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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Constructor
        public EditTransactionViewModel(TransactionHeadListEntity item, bool newRecord, string itemName) : base(item, newRecord, itemName)
        {
            TransactionDate = Item.Head.Date;

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
        private DateTime _transactionDate;
        public DateTime TransactionDate
        {
            get { return _transactionDate; }
            set { SetProperty(ref _transactionDate, value); }
        }

        // Partners
        private List<PartnerEntity> _partners;
        public List<PartnerEntity> Partners
        {
            get { return _partners; }
            set { SetProperty(ref _partners, value); }
        }

        private PartnerEntity _selectedPartner;
        public PartnerEntity SelectedPartner
        {
            get { return _selectedPartner; }
            set { SetProperty(ref _selectedPartner, value); }
        }

        // Products
        private List<ProductCategoryEntity> _productCategories;
        public List<ProductCategoryEntity> ProductCategories
        {
            get
            {
                if (_productCategories == null) _productCategories = new List<ProductCategoryEntity>();
                return _productCategories;
            }
            set { SetProperty(ref _productCategories, value); }
        }

        private ProductCategoryEntity _selectedProductCategory;
        public ProductCategoryEntity SelectedProductCategory
        {
            get { return _selectedProductCategory; }
            set
            {
                if (_selectedProductCategory != value) Products = ManageProducts.ListProducts(value.Id);
                SetProperty(ref _selectedProductCategory, value);
            }
        }

        private List<ProductListEntity> _products;
        public List<ProductListEntity> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        private ProductListEntity _selectedProduct;
        public ProductListEntity SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (value != null) ProductPrice = (Item.Head.Incoming ? value.CostPrice : value.SellPrice);
                SetProperty(ref _selectedProduct, value);
            }
        }

        private decimal _productQuantity;
        public decimal ProductQuantity
        {
            get { return _productQuantity; }
            set { SetProperty(ref _productQuantity, value); }
        }

        private decimal _productPrice;
        public decimal ProductPrice
        {
            get { return _productPrice; }
            set { SetProperty(ref _productPrice, value); }
        }

        // Transaction body
        private ObservableCollection<BindableTransactionBodyListEntity> _transactionBody;
        public ObservableCollection<BindableTransactionBodyListEntity> TransactionBody
        { 
            get
            {
                if (_transactionBody == null) _transactionBody = new ObservableCollection<BindableTransactionBodyListEntity>();
                return _transactionBody;
            }
            set { SetProperty(ref _transactionBody, value); }
        }

        private BindableTransactionBodyListEntity _selectedBody;
        public BindableTransactionBodyListEntity SelectedBody
        {
            get { return _selectedBody; }
            set { SetProperty(ref _selectedBody, value); }
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            decimal sum = 0m;
            foreach (var record in TransactionBody)
                sum += record.SumPrice;
            TotalPrice = sum;
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { SetProperty(ref _totalPrice, value); }
        }

        private ICommand _addProductCommand;
        public ICommand AddProductCommand
        {
            get
            {
                if (_addProductCommand == null) _addProductCommand = new RelayCommand(new Action<object>(AddProduct), new Predicate<object>(CanAddProduct));
                return _addProductCommand;
            }
            set { SetProperty(ref _addProductCommand, value); }
        }
        private void AddProduct(object parameter)
        {
            var rec = new TransactionBodyListEntity();
            rec.Product = new ProductEntity();
            rec.Product.Id = SelectedProduct.Id;
            rec.Product.Name = SelectedProduct.Name;
            rec.Product.Code = SelectedProduct.Code;
            rec.Product.CostPrice = SelectedProduct.CostPrice;
            rec.Product.SellPrice = SelectedProduct.SellPrice;
            rec.Body = new TransactionBodyEntity();
            rec.Body.ProductId = SelectedProduct.Id;
            rec.Body.Price = ProductPrice;
            rec.Body.Quantity = ProductQuantity;
            TransactionBody.Add(new BindableTransactionBodyListEntity(rec));
        }
        private bool CanAddProduct(object parameter)
        {
            return (SelectedProduct != null);
        }

        private ICommand _removeProductCommand;
        public ICommand RemoveProductCommand
        {
            get
            {
                if (_removeProductCommand == null) _removeProductCommand = new RelayCommand(new Action<object>(RemoveProduct), new Predicate<object>(CanRemoveProduct));
                return _removeProductCommand;
            }
            set { SetProperty(ref _removeProductCommand, value); }
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
            log.Debug("Save " + ItemName);

            if (SelectedPartner == null)
            {
                NotificationProvider.Error("Save transaction error", "Pleace select a partner.");
                return false;
            }
            else
            {
                Item.Head.PartnerId = SelectedPartner.Id;
                Item.Head.TotalPrice = TotalPrice;
                Item.Head.Date = TransactionDate;
                Item.Partner = SelectedPartner;
                var list = new List<TransactionBodyListEntity>();
                foreach (var record in TransactionBody)
                    list.Add(record.Item);
                return ManageTransactions.AddOrModifyTransaction(Item.Head, list);
            }
        }
    }
}
