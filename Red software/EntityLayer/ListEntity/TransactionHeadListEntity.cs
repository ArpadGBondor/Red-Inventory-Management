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
        public TransactionHeadListEntity(TransactionHeadEntity _Head, PartnerEntity _Partner, decimal _ListVariable)
            : this(_Head,_Partner)
        {
            ListVariable = _ListVariable;
        }

        public TransactionHeadEntity Head { get; set; }
        public PartnerEntity Partner { get; set; }

        public DateTime Date
        {
            get
            {
                DateTime date;
                if (Head == null || Head.Date == null || !DateTime.TryParse(Head.Date,out date)) return new DateTime();
                return date;
            }
            set
            {
                if (Head == null) return;
                Head.Date = value.ToString("d");
            }
        }

        public decimal ListVariable { get; set; }

        public int CompareTo(TransactionHeadListEntity other)
        {
            if (Head == null)
                return Partner.CompareTo(other.Partner);
            return Date.CompareTo(other.Date);
        }
    }
}