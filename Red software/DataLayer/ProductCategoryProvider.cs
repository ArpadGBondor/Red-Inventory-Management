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
            Database.InitializeTable<ProductCategoryEntity>();

        }

        public static int Get_ID(string category)
        {
            int id = 0;

            if (!string.IsNullOrWhiteSpace(category))
            {
                MyDataContext db = new MyDataContext(Database.ConnectionString);
                Table<ProductCategoryEntity> EntityTable = db.GetTable<ProductCategoryEntity>();
                var query =  EntityTable.Where(p=>p.Category == category);
                try { id = query.First().Id; }
                catch
                {
                    EntityTable.InsertOnSubmit(new ProductCategoryEntity() { Category = category });
                    try
                    {
                        db.SubmitChanges();
                        try { id = query.First().Id; }
                        catch { }
                    }
                    catch { }
                }
            }
            return id;
        }

        public static bool Add(ProductCategoryEntity category)
        {
            if (category == null)
                return false;
            if (string.IsNullOrWhiteSpace(category.Category))
                return false;
            if (Database.IsExist<ProductCategoryEntity>(p => p.Category == category.Category)) 
                return false;
            return Database.Add<ProductCategoryEntity>(category);
        }

        public static bool Modify(ProductCategoryEntity category)
        {
            if (category == null)
                return false;
            if (string.IsNullOrWhiteSpace(category.Category))
                return false;
            if (Database.IsExist<ProductCategoryEntity>(p => p.Id != category.Id && p.Category == category.Category))
                return false;
            return Database.Modify<ProductCategoryEntity>(category, p => p.Id == category.Id);
        }

        public static bool Remove(ProductCategoryEntity category)
        {
            if (category == null)
                return false;
            if (string.IsNullOrWhiteSpace(category.Category))
                return false;
            if (ProductProvider.IsExist(p => p.Category_Id == category.Id))
                return false;
            return Database.Remove<ProductCategoryEntity>(p => p.Id == category.Id);
        }

        public static List<ProductCategoryEntity> List(Expression<Func<ProductCategoryEntity, bool>> condition)
        {
            return Database.ListTable<ProductCategoryEntity>(condition);
        }
    }
}
