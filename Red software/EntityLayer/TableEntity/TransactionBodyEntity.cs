using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Table("TransactionBody")]
    public class TransactionBodyEntity
    {
        [Key]
        public int Id { get; set; }

        public int TransactionId { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public int ProductId { get; set; }
    }
}
