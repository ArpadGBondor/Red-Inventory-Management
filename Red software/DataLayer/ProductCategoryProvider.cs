using System.Collections.Generic;
using EntityLayer;
using System.Linq.Expressions;
using System;
using System.Data.Linq;
using System.Linq;

namespace DataLayer
{
    public class ProductCategoryProvider
    {
        static ProductCategoryProvider()
        {
            Database.InitializeTable(typeof(ProductCategoryEntity));
            Database.InitializeTable(typeof(ProductEntity));
        }

        public static int Get_ID(string category)
        {
            int id = 0;

            if (!string.IsNullOrWhiteSpace(category))
                if (Database.OpenConnection())
                {
                    DataContext db = new DataContext(Database.get_connectionString);
                    Table<ProductCategoryEntity> EntityTable = db.GetTable<ProductCategoryEntity>();
                    var query = EntityTable.Where(p=>p.Category == category);
                    foreach (var rec in query)
                    {
                        id = rec.Id;
                    }
                    if (id == 0)
                    {
                        EntityTable.InsertOnSubmit(new ProductCategoryEntity() { Category = category });
                        try
                        {
                            db.SubmitChanges();
                            foreach (var rec in query)
                            {
                                id = rec.Id;
                            }
                        }
                        catch { }
                    }
                    Database.CloseConnection();
                }
            return id;
        }

        public static bool Add(ProductCategoryEntity category)
        {
            if (string.IsNullOrWhiteSpace(category.Category) || Database.IsExist<ProductCategoryEntity>(p=>p.Category == category.Category))
            {
                return false;
            }
            return Database.Add<ProductCategoryEntity>(category);
        }

        public static bool Modify(ProductCategoryEntity category)
        {
            if (string.IsNullOrWhiteSpace(category.Category) || Database.IsExist<ProductCategoryEntity>(p => p.Category == category.Category))
            {
                return false;
            }
            return Database.Modify<ProductCategoryEntity>(category, p => p.Id == category.Id);
        }

        public static bool Remove(ProductCategoryEntity category)
        {
            if (string.IsNullOrWhiteSpace(category.Category) || Database.IsExist<ProductEntity>(p => p.Category_Id == category.Id))
            {
                return false;
            }
            return Database.Remove<ProductCategoryEntity>(p => p.Id == category.Id);
        }

        public static List<ProductCategoryEntity> List(Expression<Func<ProductCategoryEntity, bool>> condition)
        {
            return Database.ListTable<ProductCategoryEntity>(condition);
        }
    }
}
