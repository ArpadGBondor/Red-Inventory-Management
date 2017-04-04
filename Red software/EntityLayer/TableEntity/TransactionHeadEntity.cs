using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Table(Name = "TransactionHeader")]
    public class TransactionHeadEntity
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column]
        public bool Incoming { get; set; }

        [Column]
        public int Partner_Id { get; set; }

        [Column]
        public string Date { get; set; }

        [Column]
        public decimal TotalPrice { get; set; }
    }
}