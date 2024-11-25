using System;

namespace EntityLayer
{
    public class TransactionHeadListEntity : IComparable<TransactionHeadListEntity>
    {
        public TransactionHeadListEntity() { }
        public TransactionHeadListEntity(TransactionHeadEntity head, PartnerEntity partner)
            : this()
        {
            Head = head;
            Partner = partner;
        }
        public TransactionHeadListEntity(TransactionHeadEntity head, PartnerEntity partner, decimal listVariable)
            : this(head, partner)
        {
            ListVariable = listVariable;
        }

        public TransactionHeadEntity Head { get; set; }
        public PartnerEntity Partner { get; set; }

        public decimal ListVariable { get; set; }

        public int CompareTo(TransactionHeadListEntity other)
        {
            if (Head == null)
                return Partner.CompareTo(other.Partner);
            return Head.Date.CompareTo(other.Head.Date);
        }
    }
}