using System.Collections.Generic;
using EntityLayer;

namespace DataLayer
{
    public class ProductProvider
    {
        static ProductProvider()
        {
            Database.InitializeTable(typeof(ProductEntity));
        }

        public static bool Add(ProductEntity product)
        {
            return Database.Add<ProductEntity>(product);
        }

        public static bool Modify(ProductEntity product)
        {
            return Database.Modify<ProductEntity>(product, p => p.Id == product.Id);
        }

        public static bool Remove(ProductEntity product)
        {
            return Database.Remove<ProductEntity>(p => p.Id == product.Id);
        }
        public static List<ProductEntity> List()
        {
            return Database.ListTable<ProductEntity>(p => true);
        }
    }
}
