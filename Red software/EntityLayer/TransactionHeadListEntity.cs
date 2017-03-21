namespace EntityLayer
{
    public class TransactionHeadListEntity
    {
        public TransactionHeadListEntity() { }
        public TransactionHeadListEntity(TransactionHeadEntity _Head, PartnerEntity _Partner)
            :this()
        {
            Head = _Head;
            Partner = _Partner;
        }
        TransactionHeadEntity Head { get; set; }
        PartnerEntity Partner { get; set; }
    }
}