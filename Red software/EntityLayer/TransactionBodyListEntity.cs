namespace EntityLayer
{
    public class TransactionBodyListEntity
    {
        public TransactionBodyListEntity() { }
        public TransactionBodyListEntity(TransactionBodyEntity _Body, ProductEntity _Product) 
            :this()
        {
            Body = _Body;
            Product = _Product;
        }
        public TransactionBodyEntity Body { get; set; }
        public ProductEntity Product { get; set; }

        public decimal SumPrice
        {
            get
            {
                if (Body == null) return 0;
                return Body.Price * Body.Quantity;
            }
        }
    }
}
