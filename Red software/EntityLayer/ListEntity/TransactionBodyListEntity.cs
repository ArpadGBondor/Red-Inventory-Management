namespace EntityLayer
{
    public class TransactionBodyListEntity
    {
        public TransactionBodyListEntity() { }
        public TransactionBodyListEntity(TransactionBodyEntity body, ProductEntity product) 
            :this()
        {
            Body = body;
            Product = product;
        }
        public TransactionBodyEntity Body { get; set; }
        public ProductEntity Product { get; set; }

    }
}
