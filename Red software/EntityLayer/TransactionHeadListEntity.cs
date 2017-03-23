using System;

namespace EntityLayer
{
    public class TransactionHeadListEntity : IComparable<TransactionHeadListEntity>
    {
        public TransactionHeadListEntity() { }
        public TransactionHeadListEntity(TransactionHeadEntity _Head, PartnerEntity _Partner)
            :this()
        {
            Head = _Head;
            Partner = _Partner;
        }
        public TransactionHeadEntity Head { get; set; }
        public PartnerEntity Partner { get; set; }

        public int CompareTo(TransactionHeadListEntity other)
        {
            return DateTime.Parse(this.Head.Date).CompareTo(DateTime.Parse(other.Head.Date));
        }
    }
}