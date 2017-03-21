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
        TransactionBodyEntity Body { get; set; }
        ProductEntity Product { get; set; }
    }
}
