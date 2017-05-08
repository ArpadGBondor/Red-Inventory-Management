using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer
{
    [Table("TransactionHeader")]
    public class TransactionHeadEntity
    {
        [Key]
        public int Id { get; set; }

        public bool Incoming { get; set; }

        public int PartnerId { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime Date { get; set; }

        public decimal TotalPrice { get; set; }
    }
}