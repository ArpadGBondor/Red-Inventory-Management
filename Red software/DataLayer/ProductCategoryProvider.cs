using System.Collections.Generic;
using EntityLayer;

namespace DataLayer
{
    public class ProductCategoryProvider
    {
        static ProductCategoryProvider()
        {
            Database.InitializeTable(typeof(ProductCategoryEntity));
        }



        public static bool Add(ProductCategoryEntity product)
        {
            return Database.Add<ProductCategoryEntity>(product);
        }

        public static bool Modify(ProductCategoryEntity product)
        {
            return Database.Modify<ProductCategoryEntity>(product, p => p.Id == product.Id);
        }

        public static bool Remove(ProductCategoryEntity product)
        {
            return Database.Remove<ProductCategoryEntity>(p => p.Id == product.Id);
        }

        public static List<ProductCategoryEntity> List()
        {
            return Database.ListTable<ProductCategoryEntity>(p => true);
        }
    }
}
