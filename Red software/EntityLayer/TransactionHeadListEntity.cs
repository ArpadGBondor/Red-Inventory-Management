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

        public int CompareTo(TransactionHeadListEntity other)
        {
            return Date.CompareTo(Date);
        }
    }
}