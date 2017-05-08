using System.Collections.Generic;
using EntityLayer;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace DataLayer
{
    public class ProductCategoryProvider
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the Id of the product category in the parameter.
        /// If the category does not exist, the function adds 
        /// a new record to the database, and returns the new record's Id.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static int GetId(string category)
        {
            int id = 0;

            if (!string.IsNullOrWhiteSpace(category))
            {
                using (var db = new InventoryContext(DatabaseConnection.ConnectionString))
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var query = db.ProductCategories.Where(p => p.Category == category);
                            var record = query.FirstOrDefault();

                            if (record == null)
                            {
                                db.ProductCategories.Add(new ProductCategoryEntity() { Category = category });
                                db.SaveChanges();
                                record = query.FirstOrDefault();
                            }
                            id = record.Id;
                            
                            dbTransaction.Commit();                         
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            log.Error("Cannot add a category to the database.\nCategory name: " + category, ex);
                            throw ex;
                        }
                    }
                }
            }
            return id;
        }

        /// <summary>
        /// Adds a new product category to the database
        /// The function cannot add a category if
        ///     the parameter is null, or
        ///     the category name is empty, or
        ///     the category name already exists.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static bool Add(ProductCategoryEntity category)
        {
            if ((category == null)
                || (string.IsNullOrWhiteSpace(category.Category))
                || (DatabaseConnection.IsExist<ProductCategoryEntity>(p => p.Category == category.Category))) 
                return false;
            return DatabaseConnection.Add<ProductCategoryEntity>(category);
        }

        /// <summary>
        /// Modifies a product category in the database.
        /// The function cannot modify a category if
        ///     the parameter is null, or
        ///     the new category name is empty, or
        ///     the new category name already exists with different Id.
        /// </summary>
        /// <param name="category">The Id property selects the category.</param>
        /// <returns></returns>
        public static bool Modify(ProductCategoryEntity category)
        {
            if ((category == null)
                || (string.IsNullOrWhiteSpace(category.Category))
                || (DatabaseConnection.IsExist<ProductCategoryEntity>(p => p.Id != category.Id && p.Category == category.Category)))
                return false;
            return DatabaseConnection.Modify<ProductCategoryEntity>(category, p => p.Id == category.Id);
        }

        /// <summary>
        /// Removes a product category from the database.
        /// The function cannot remove a product category if
        ///     the parameter is null, or
        ///     the product category is set to one or more product.
        /// </summary>
        /// <param name="category">The Id property selects the category.</param>
        /// <returns></returns>
        public static bool Remove(ProductCategoryEntity category)
        {
            if ((category == null)
                || (ProductProvider.IsExist(p => p.CategoryId == category.Id)))
                return false;
            return DatabaseConnection.Remove<ProductCategoryEntity>(p => p.Id == category.Id);
        }

        /// <summary>
        /// Lists every record from the ProdCategory table where the condition returns true
        /// </summary>
        /// <param name="condition">Condition on the records of the table. Eg. (p => p.Id == record.Id)</param>
        /// <returns></returns>
        public static List<ProductCategoryEntity> List(Expression<Func<ProductCategoryEntity, bool>> condition)
        {
            return DatabaseConnection.ListTable<ProductCategoryEntity>(condition);
        }
    }
}
