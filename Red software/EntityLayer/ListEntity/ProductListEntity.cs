using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class ProductListEntity : IComparable<ProductListEntity>
    {
        public ProductListEntity() { }
        public ProductListEntity(ProductEntity p)
            :this()
        {
            Code = p.Code;
            Cost_Price = p.Cost_Price;
            Id = p.Id;
            Name = p.Name;
            Sell_Price = p.Sell_Price;
        }
        public ProductListEntity(ProductEntity p, ProductCategoryEntity c)
            :this(p)
        {
            Category = c.Category;
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Cost_Price { get; set; }
        public decimal Sell_Price { get; set; }

        public int CompareTo(ProductListEntity other)
        {
            var compare1 = (Name != null ? Name : "");
            var compare2 = (other.Name != null ? other.Name : "");
            return compare1.CompareTo(compare2);
        }
    }
}
