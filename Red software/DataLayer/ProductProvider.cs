using System.Collections.Generic;
using EntityLayer;
using System.Data.Linq;
using System.Linq;
using System;
using System.Linq.Expressions;

namespace DataLayer
{
    public class ProductProvider
    {
        static ProductProvider()
        {
            Database.InitializeTable<ProductEntity>();
            Database.InitializeTable<ProductCategoryEntity>();
        }

        private static ProductEntity ConvertToProduct(ProductListEntity parameter)
        {
            ProductEntity record = new ProductEntity();

            record.Category_Id  = ProductCategoryProvider.Get_ID(parameter.Category);
            record.Code         = parameter.Code;
            record.Cost_Price   = parameter.Cost_Price;
            record.Id           = parameter.Id;
            record.Name         = parameter.Name;
            record.Sell_Price   = parameter.Sell_Price;

            return record;
        }
        public static bool Add(ProductListEntity product)
        {
            if (product == null)
                return false;
            return Database.Add<ProductEntity>(ConvertToProduct(product));
        }

        public static bool Modify(ProductListEntity product)
        {
            if (product == null)
                return false;
            return Database.Modify<ProductEntity>(ConvertToProduct(product),p=>p.Id==product.Id);
        }

        public static bool Remove(ProductListEntity product)
        {
            if (product == null)
                return false;
            if (TransactionProvider.IsExistBody(p => p.Product_Id == product.Id))
                return false;
            return Database.Remove<ProductEntity>(p => p.Id == product.Id);
        }

        public static List<ProductListEntity> List(Expression<Func<ProductEntity, bool>> condition)
        {
            List<ProductListEntity> list = new List<ProductListEntity>();

            MyDataContext db = new MyDataContext(Database.ConnectionString);

            var query1 = (from p in db.Products.Where(condition)
                            join c in db.ProductCategories on p.Category_Id equals c.Id into CatJoin
                            from subc in CatJoin.DefaultIfEmpty()
                            select new { Product = p, Category = subc });
            foreach(var rec in query1)
            {
                if (rec.Category == null)
                    list.Add(new ProductListEntity(rec.Product));
                else
                    list.Add(new ProductListEntity(rec.Product, rec.Category));
            }
            list.Sort();
            return list;
        }

        public static bool IsExist(Expression<Func<ProductEntity, bool>> condition)
        {
            return Database.IsExist<ProductEntity>(condition);
        }
    }
}
