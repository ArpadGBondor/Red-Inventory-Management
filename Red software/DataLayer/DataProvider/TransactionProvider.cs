using System.Collections.Generic;
using EntityLayer;
using System.Linq;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DataLayer
{
    public class TransactionProvider
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Lists every transaction's header record from the TransactionHeader table where the condition returns true.
        /// </summary>
        /// <param name="condition">Condition on the records of the table. Eg. (p => p.Id == record.Id)</param>
        /// <returns></returns>
        public static List<TransactionHeadListEntity> ListHead(Expression<Func<TransactionHeadEntity,bool>> condition)
        {
            List<TransactionHeadListEntity> list = new List<TransactionHeadListEntity>();
            using (var db = new InventoryContext(DatabaseConnection.ConnectionString))
            {
                // The TransactionHeadListEntity contains the 
                // header record from the TransactionHeader table
                // and the partner record from the Partners table.
                var query = (from h in db.TransactionHeader.Where(condition)
                             join p in db.Partners on h.PartnerId equals p.Id into PartJoin
                             from subp in PartJoin.DefaultIfEmpty()
                             select new { Head = h, Partner = subp });
                foreach (var record in query)
                    list.Add(new TransactionHeadListEntity(record.Head, record.Partner) );
                list.Sort();
            }
            return list;
        }

        /// <summary>
        /// Lists every transaction's body records from the TransactionBody table where the condition returns true.
        /// </summary>
        /// <param name="condition">Condition on the records of the table. Eg. (p => p.Id == record.Id)</param>
        /// <returns></returns>
        public static List<TransactionBodyListEntity> ListBody(Expression<Func<TransactionBodyEntity, bool>> condition)
        {
            List<TransactionBodyListEntity> list = new List<TransactionBodyListEntity>();
            using (var db = new InventoryContext(DatabaseConnection.ConnectionString))
            {
                // The TransactionBodyListEntity contains the 
                // body record from the TransactionBody table
                // and the product record from the Products table.
                var query = (from b in db.TransactionBody.Where(condition)
                             join p in db.Products on b.ProductId equals p.Id into ProdJoin
                             from subp in ProdJoin.DefaultIfEmpty()
                             select new { Body = b, Product = subp});
                foreach (var record in query)
                    list.Add(new TransactionBodyListEntity(record.Body, record.Product));
            }
            return list;
        }

        /// <summary>
        /// Lists every products's stock quantity from the TransactionBody table where the condition returns true.
        /// </summary>
        /// <param name="condition">Condition on the records of the table. Eg. (p => p.Id == record.Id)</param>
        /// <returns></returns>
        public static List<TransactionBodyListEntity> ListInventory(Expression<Func<TransactionBodyEntity, bool>> condition)
        {
            List<TransactionBodyListEntity> list = new List<TransactionBodyListEntity>();
            using (var db = new InventoryContext(DatabaseConnection.ConnectionString))
            {
                // The first query collects the product IDs and the 
                // quantity changes of every record from the TransactionBody table.
                var query1 = (from body in db.TransactionBody.Where(condition)
                              join head in db.TransactionHeader on body.TransactionId equals head.Id
                              select new { ProductId = body.ProductId, Quantity = body.Quantity * (head.Incoming ? 1 : -1) });
                // The secound query groups the first query's result by ProductID and 
                // sums the quantity changes to get the stock quantity of the product.
                // It also returns the product's record from the Products table.
                var query2 = (from q1 in query1
                              group q1 by q1.ProductId into g
                              join product in db.Products on g.FirstOrDefault().ProductId equals product.Id
                              select new { Quantity = g.Sum(p => p.Quantity), Product = product });
                foreach (var record in query2)
                {
                    list.Add(
                        new TransactionBodyListEntity(
                            new TransactionBodyEntity() { Quantity = record.Quantity },
                            record.Product));
                }
            }
            return list;
        }

        /// <summary>
        /// Lists every transaction that changes the stock quantity of the product in the parameter.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<TransactionHeadListEntity> ListInventoryDetails(int productId)
        {
            List<TransactionHeadListEntity> list = new List<TransactionHeadListEntity>();
            using (var db = new InventoryContext(DatabaseConnection.ConnectionString))
            {
                // The query selects every record from the TransactionBody table, 
                // that alters the stock quantity of the product in the parameter of the function.
                // It groups the records by TransactionId, so the result will contain only one record per transaction.
                // The query returns the details of each transaction that alters the stock quantity of the product, 
                // and sums the quantity changes of the product in the transaction.
                var query = (from body in db.TransactionBody.Where(p => p.ProductId == productId)
                             group body by body.TransactionId into g
                             join head in db.TransactionHeader on g.FirstOrDefault().TransactionId equals head.Id
                             join partner in db.Partners on head.PartnerId equals partner.Id
                             select new { Head = head, Partner = partner, Sum = g.Sum(p => p.Quantity) * (head.Incoming ? 1 : -1) } );

                foreach (var record in query)
                    list.Add(new TransactionHeadListEntity(record.Head, record.Partner, record.Sum));
            }
            return list;
        }

        /// <summary>
        /// Lists every partner that has transactions, and sums the total price of their transactions.
        /// </summary>
        /// <param name="condition">Condition on the records of the TransactionHeader table. Eg. (p => true)</param>
        /// <returns></returns>
        public static List<TransactionHeadListEntity> ListPartnerTransactions(Expression<Func<TransactionHeadEntity, bool>> condition)
        {
            List<TransactionHeadListEntity> list = new List<TransactionHeadListEntity>();
            using (var db = new InventoryContext(DatabaseConnection.ConnectionString))
            {
                // The query groups the transaction headers by partners 
                // and sums the total price of their transactions.
                var query = (from head in db.TransactionHeader.Where(condition)
                             group head by head.PartnerId into g
                             join partner in db.Partners on g.FirstOrDefault().PartnerId equals partner.Id
                             select new { Partner = partner, Sum = g.Sum(p => p.TotalPrice * (p.Incoming ? -1 : 1)) });
                foreach (var record in query)
                    list.Add(new TransactionHeadListEntity(null, record.Partner, record.Sum));
            }
            return list;
        }

        /// <summary>
        /// Checks if there is at least one record in the TransactionHeader table where the condition returns true.
        /// </summary>
        /// <param name="condition">Condition on the records of the table. Eg. (p => p.Id == record.Id)</param>
        /// <returns></returns>
        public static bool IsExistHead(Expression<Func<TransactionHeadEntity, bool>> condition)
        {
            return DatabaseConnection.IsExist<TransactionHeadEntity>(condition);
        }

        /// <summary>
        /// Checks if there is at least one record in the TransactionBody table where the condition returns true.
        /// </summary>
        /// <param name="condition">Condition on the records of the table. Eg. (p => p.Id == record.Id)</param>
        /// <returns></returns>
        public static bool IsExistBody(Expression<Func<TransactionBodyEntity, bool>> condition)
        {
            return DatabaseConnection.IsExist<TransactionBodyEntity>(condition);
        }

        /// <summary>
        /// Adds or Modifies a transaction in the database.
        /// </summary>
        /// <param name="head">If the Id property equals 0, the function adds a new transaction. If there is a transaction with the same Id in the database, the function modifies it.</param>
        /// <param name="body"></param>
        /// <returns>If the function was successfull, it returns true.</returns>
        public static bool AddOrModifyTransaction(TransactionHeadEntity head, List<TransactionBodyListEntity> body)
        {
            if (head == null || body == null)
                return false;
            bool result = false;
            bool newHead = (head.Id == 0);

            using (var db = new InventoryContext(DatabaseConnection.ConnectionString))
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        int transactionId = head.Id;

                        // HEAD record
                        if (!newHead) // Modify
                        {
                            var headQuery = db.TransactionHeader.Where(p => p.Id == head.Id);
                            foreach (var record in headQuery)
                            {
                                PropertyInfo[] properties = EntityCloner.GetProperties(typeof(TransactionHeadEntity));
                                foreach (PropertyInfo property in properties)
                                {
                                    property.SetValue(record, property.GetValue(head));
                                }
                            }
                            db.SaveChanges();
                            result = true;
                        }
                        else // Insert
                        {
                            db.TransactionHeader.Add(head);
                            db.SaveChanges();
                            result = true;
                            transactionId = head.Id;
                        }

                        if (result)
                        {
                            // BODY records
                            result = false;
                            // Set transaction ID
                            foreach (var rec in body)
                            {
                                rec.Body.TransactionId = transactionId;
                            }

                            // Delete or modify existing records
                            var query = db.TransactionBody.Where(p => p.TransactionId == transactionId);
                            foreach (var rec in query)
                            {
                                var newrec = body.Where(p => p.Body.Id == rec.Id).FirstOrDefault();
                                if (newrec == null)
                                {
                                    db.TransactionBody.Remove(rec);
                                }
                                else
                                {
                                    PropertyInfo[] properties = EntityCloner.GetProperties(typeof(TransactionBodyEntity));
                                    foreach (PropertyInfo property in properties)
                                    {
                                        property.SetValue(rec, property.GetValue(newrec.Body));
                                    }
                                    body.Remove(newrec);
                                }
                            }

                            // Insert new records
                            foreach (var rec in body)
                            {
                                db.TransactionBody.Add(rec.Body);
                            }

                            db.SaveChanges();
                            dbTransaction.Commit();
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        log.Error("Cannot add or modify a transaction.", ex);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Removes every transaction from the TransactionHeader and TransactionBody tables where the condition returns true.
        /// </summary>
        /// <param name="condition">Condition on the records of the TransactionHeader table. Eg. (p => p.Id == record.Id)</param>
        /// <returns></returns>
        public static bool RemoveTransaction(Expression<Func<TransactionHeadEntity, bool>> condition)
        {
            bool result = false;
            List<int> DeletedTransactions = new List<int>();
            using (var db = new InventoryContext(DatabaseConnection.ConnectionString))
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Delete head
                        var query = db.TransactionHeader.Where(condition);
                        foreach (var rec in query)
                        {
                            DeletedTransactions.Add(rec.Id);
                            db.TransactionHeader.Remove(rec);
                        }

                        // Delete body
                        foreach (var rec in db.TransactionBody)
                        {
                            foreach (var DeletedId in DeletedTransactions)
                            {
                                if (DeletedId == rec.TransactionId)
                                {
                                    db.TransactionBody.Remove(rec);
                                }
                            }
                        }

                        db.SaveChanges();
                        dbTransaction.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        log.Error("Cannot remove a transaction.", ex);
                    }
                }
                return result;
            }
        }
    }
}
