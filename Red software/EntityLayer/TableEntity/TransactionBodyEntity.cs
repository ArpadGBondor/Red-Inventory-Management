using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Table(Name = "TransactionBody")]
    public class TransactionBodyEntity
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column]
        public int Transaction_Id { get; set; }

        [Column]
        public decimal Quantity { get; set; }

        [Column]
        public decimal Price { get; set; }

        [Column]
        public int Product_Id { get; set; }
    }
}
