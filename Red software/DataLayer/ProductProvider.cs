using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using System.Data.Linq;
using System.Reflection;

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
            bool lSuccess = false;

            if (Database.OpenConnection())
            {
                DataContext db = new DataContext(Database.get_connectionString);
                Table<ProductEntity> Products = db.GetTable<ProductEntity>();
                Products.InsertOnSubmit(product);
                try
                {
                    db.SubmitChanges();
                    lSuccess = true;
                }
                catch
                {

                }
                Database.CloseConnection();
            }
            return lSuccess;
        }

        public static bool Modify(ProductEntity product)
        {
            bool lSuccess = false;

            if (Database.OpenConnection())
            {
                DataContext db = new DataContext(Database.get_connectionString);
                Table<ProductEntity> Products = db.GetTable<ProductEntity>();
                var q = (from p in Products
                         where p.Id == product.Id
                         select p);
                foreach(var p in q)
                {
                    lSuccess = true;

                    EntityCloner.CloneProperties(p, product);

                }
                try
                {
                    db.SubmitChanges();
                    lSuccess = true;
                }
                catch
                {

                }
                Database.CloseConnection();
            }
            return lSuccess;
        }

        public static bool Remove(ProductEntity product)
        {
            bool lSuccess = false;
            bool lExist = false;

            if (Database.OpenConnection())
            {
                DataContext db = new DataContext(Database.get_connectionString);
                Table<ProductEntity> Products = db.GetTable<ProductEntity>();
                var q = (from p in Products
                         where p.Id == product.Id
                         select p);
                foreach (var p in q)
                {
                    Products.DeleteOnSubmit(p);
                    lExist = true;
                }               
                if (lExist)
                {
                    try
                    {
                        db.SubmitChanges();
                        lSuccess = true;
                    }
                    catch
                    {

                    }
                }
                Database.CloseConnection();
            }
            return lSuccess;
        }
        public static List<ProductEntity> List()
        {
            return Database.ListTable<ProductEntity>();
        }
    }
}
