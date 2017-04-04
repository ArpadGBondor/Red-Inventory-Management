using System;
using System.Data.Linq.Mapping;

namespace EntityLayer
{
    [Table(Name = "Products")]
    public class ProductEntity 
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column]
        public string Code { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public int Category_Id { get; set; }

        [Column]
        public decimal Cost_Price { get; set; }

        [Column]
        public decimal Sell_Price { get; set; }

    }
}