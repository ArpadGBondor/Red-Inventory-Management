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
            Database.InitializeTable(typeof(ProductEntity));
            Database.InitializeTable(typeof(ProductCategoryEntity));
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
            return Database.Add<ProductEntity>(ConvertToProduct(product));
        }

        public static bool Modify(ProductListEntity product)
        {
            return Database.Modify<ProductEntity>(ConvertToProduct(product),p=>p.Id==product.Id);
        }

        public static bool Remove(ProductListEntity product)
        {
            return Database.Remove<ProductEntity>(p => p.Id == product.Id);
        }
        public static List<ProductListEntity> List(Expression<Func<ProductEntity, bool>> condition)
        {
            List<ProductListEntity> list = new List<ProductListEntity>();
            if (Database.OpenConnection())
            {
                DataContext db = new DataContext(Database.get_connectionString);
                Table<ProductEntity> ProductTable = db.GetTable<ProductEntity>();
                Table<ProductCategoryEntity> CategoryTable = db.GetTable<ProductCategoryEntity>();


                var query1 = (from p in ProductTable.Where(condition)
                              join c in CategoryTable on p.Category_Id equals c.Id into CatJoin
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
                Database.CloseConnection();
            }
            return list;
        }

        public static bool ChangeCategory(int from,int to)
        {
            bool lSuccess = false;
            bool lExist = false;

            if (Database.OpenConnection())
            {
                DataContext db = new DataContext(Database.get_connectionString);
                Table<ProductEntity> EntityTable = db.GetTable<ProductEntity>();
                var query = EntityTable.Where(p => p.Category_Id == from); 
                foreach (var rec in query)
                {
                    lExist = true;
                    rec.Category_Id = to;
                }
                if (lExist)
                {
                    try
                    {
                        db.SubmitChanges();
                        lSuccess = true;
                    }
                    catch /*(Exception e)*/
                    {
                        // Cannot modify primary key
                    }
                }
                Database.CloseConnection();
            }
            return lSuccess;
        }
    }
}
