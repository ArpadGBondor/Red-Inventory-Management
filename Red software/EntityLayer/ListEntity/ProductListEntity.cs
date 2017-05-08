using System;

namespace EntityLayer
{
    public class ProductListEntity : IComparable<ProductListEntity>
    {
        public ProductListEntity() { }
        public ProductListEntity(ProductEntity p)
            :this()
        {
            Code = p.Code;
            CostPrice = p.CostPrice;
            Id = p.Id;
            Name = p.Name;
            SellPrice = p.SellPrice;
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
        public decimal CostPrice { get; set; }
        public decimal SellPrice { get; set; }

        public int CompareTo(ProductListEntity other)
        {
            var compare1 = Name ?? string.Empty;
            var compare2 = other.Name ?? string.Empty;
            return compare1.CompareTo(compare2);
        }
    }
}
