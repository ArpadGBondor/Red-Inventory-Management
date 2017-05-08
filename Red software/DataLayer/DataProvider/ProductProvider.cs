using System.Collections.Generic;
using EntityLayer;
using System.Linq;
using System;
using System.Linq.Expressions;

namespace DataLayer
{
    public class ProductProvider
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Convert the ProductListEntity parameter into a record that can be inserted into the Products datatable.
        /// </summary>
        /// <param name="listElement"></param>
        /// <returns>Returns null if the conversion fails.</returns>
        private static ProductEntity ConvertToProduct(ProductListEntity listElement)
        {
            ProductEntity record = null;
            if (listElement != null)
            {
                try
                {
                    int categoryId = ProductCategoryProvider.GetId(listElement.Category);
                    record = new ProductEntity()
                    {
                        CategoryId = categoryId,
                        Code = listElement.Code,
                        CostPrice = listElement.CostPrice,
                        Id = listElement.Id,
                        Name = listElement.Name,
                        SellPrice = listElement.SellPrice
                    };
                }
                catch (Exception ex)
                {
                    log.Error("Cannot convert ProductListEntity to ProductEntity", ex);
                }
            }
            return record;
        }

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="product">The Id property is automatically generated.</param>
        /// <returns></returns>
        public static bool Add(ProductListEntity product)
        {
            ProductEntity record = ConvertToProduct(product);
            if (record == null)
                return false;
            return DatabaseConnection.Add<ProductEntity>(record);
        }

        /// <summary>
        /// Modifies a product in the database.
        /// </summary>
        /// <param name="product">The Id property cannot be modified.</param>
        /// <returns></returns>
        public static bool Modify(ProductListEntity product)
        {
            ProductEntity record = ConvertToProduct(product);
            if (record == null)
                return false;
            return DatabaseConnection.Modify<ProductEntity>(record, p=>p.Id== record.Id);
        }

        /// <summary>
        /// Removes a product from the database.
        /// </summary>
        /// <param name="product">The Id property selects the product.</param>
        /// <returns></returns>
        public static bool Remove(ProductListEntity product)
        {
            if ((product == null)
                || (TransactionProvider.IsExistBody(p => p.ProductId == product.Id)))
                return false;
            return DatabaseConnection.Remove<ProductEntity>(p => p.Id == product.Id);
        }

        /// <summary>
        /// Lists every record from the Products table where the condition returns true.
        /// The ProductListEntity contains the name of the category instead of CategoryID.
        /// </summary>
        /// <param name="condition">Condition on the records of the table. Eg. (p => p.Id == record.Id)</param>
        /// <returns></returns>
        public static List<ProductListEntity> List(Expression<Func<ProductEntity, bool>> condition)
        {
            List<ProductListEntity> list = new List<ProductListEntity>();
            using (var db = new InventoryContext(DatabaseConnection.ConnectionString))
            {
                var query1 = (from p in db.Products.Where(condition)
                              join c in db.ProductCategories on p.CategoryId equals c.Id into CatJoin
                              from subc in CatJoin.DefaultIfEmpty()
                              select new { Product = p, Category = subc });
                foreach (var rec in query1)
                {
                    if (rec.Category == null)
                        list.Add(new ProductListEntity(rec.Product));
                    else
                        list.Add(new ProductListEntity(rec.Product, rec.Category));
                }
                list.Sort();
            }
            return list;
        }
        /// <summary>
        /// Checks if there is at least one record in the Products table where the condition returns true.
        /// </summary>
        /// <param name="condition">Condition on the records of the table. Eg. (p => p.Id == record.Id)</param>
        /// <returns></returns>
        public static bool IsExist(Expression<Func<ProductEntity, bool>> condition)
        {
            return DatabaseConnection.IsExist<ProductEntity>(condition);
        }
    }
}
