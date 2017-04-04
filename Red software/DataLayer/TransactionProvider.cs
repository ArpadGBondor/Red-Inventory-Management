using System.Collections.Generic;
using EntityLayer;
using System.Data.Linq;
using System.Linq;
using System;
using System.Linq.Expressions;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace DataLayer
{
    public class TransactionProvider
    {
        static TransactionProvider()
        {
            Database.InitializeTable<TransactionHeadEntity>();
            Database.InitializeTable<TransactionBodyEntity>();
            Database.InitializeTable<ProductEntity>();
            Database.InitializeTable<PartnerEntity>();
        }

        public static List<TransactionHeadListEntity> ListHead(Expression<Func<TransactionHeadEntity,bool>> condition)
        {
            List<TransactionHeadListEntity> list = new List<TransactionHeadListEntity>();
            MyDataContext db = new MyDataContext(Database.ConnectionString);
            var query = (from h in db.TransactionHead.Where(condition)
                         join p in db.Partners on h.Partner_Id equals p.Id into PartJoin
                            from subp in PartJoin.DefaultIfEmpty()
                            select new TransactionHeadListEntity(h, subp));
            list.AddRange(query);
            list.Sort();
            return list;
        }

        public static List<TransactionBodyListEntity> ListBody(Expression<Func<TransactionBodyEntity, bool>> condition)
        {
            List<TransactionBodyListEntity> list = new List<TransactionBodyListEntity>();
            MyDataContext db = new MyDataContext(Database.ConnectionString);
            var query = (from b in db.TransactionBody.Where(condition)
                         join p in db.Products on b.Product_Id equals p.Id into ProdJoin
                         from subp in ProdJoin.DefaultIfEmpty()
                         select new TransactionBodyListEntity(b, subp));
            list.AddRange(query);
            return list;
        }

        public static List<TransactionBodyListEntity> ListInventory(Expression<Func<TransactionBodyEntity, bool>> condition)
        {
            List<TransactionBodyListEntity> list = new List<TransactionBodyListEntity>();
            MyDataContext db = new MyDataContext(Database.ConnectionString);
            var query1 = (from body in db.TransactionBody.Where(condition)
                         join head in db.TransactionHead on body.Transaction_Id equals head.Id
                         select new { Product_Id = body.Product_Id, Quantity = body.Quantity * (head.Incoming ? 1 :-1 ) });
            var query2 = (from q1 in query1
                          group q1 by q1.Product_Id into g
                          join product in db.Products on g.First().Product_Id equals product.Id
                          select new { Quantity = g.Sum(p => p.Quantity), Product = product });
            foreach (var record in query2)
            {
                list.Add(
                    new TransactionBodyListEntity(
                        new TransactionBodyEntity() { Quantity = record.Quantity },
                        record.Product));
            }
            return list;
        }

        public static List<TransactionHeadListEntity> ListInventoryDetails(int Product_Id)
        {
            List<TransactionHeadListEntity> list = new List<TransactionHeadListEntity>();
            MyDataContext db = new MyDataContext(Database.ConnectionString);
            var query = (from body in db.TransactionBody.Where(p => p.Product_Id == Product_Id)
                          group body by body.Transaction_Id into g
                          join head in db.TransactionHead on g.First().Transaction_Id equals head.Id
                          join partner in db.Partners on head.Partner_Id equals partner.Id
                          select new TransactionHeadListEntity(head, partner, g.Sum(p => p.Quantity) * (head.Incoming ? 1 : -1)));

            list.AddRange(query);
            return list;
        }

        public static List<TransactionHeadListEntity> ListPartnerTransactions(Expression<Func<TransactionHeadEntity, bool>> condition)
        {
            List<TransactionHeadListEntity> list = new List<TransactionHeadListEntity>();
            MyDataContext db = new MyDataContext(Database.ConnectionString);
            var query = (from head in db.TransactionHead.Where(condition)
                         group head by head.Partner_Id into g
                         join partner in db.Partners on g.First().Partner_Id equals partner.Id
                         select new TransactionHeadListEntity(null, partner, g.Sum(p => p.TotalPrice * (p.Incoming ? -1 : 1))));
            list.AddRange(query);
            return list;
        }

        public static bool IsExistHead(Expression<Func<TransactionHeadEntity, bool>> condition)
        {
            return Database.IsExist<TransactionHeadEntity>(condition);
        }

        public static bool IsExistBody(Expression<Func<TransactionBodyEntity, bool>> condition)
        {
            return Database.IsExist<TransactionBodyEntity>(condition);
        }

        public static bool AddOrModifyTransaction(TransactionHeadEntity head, List<TransactionBodyListEntity> body)
        {
            if (head == null || body == null)
                return false;
            bool lSuccess = false;
            bool lNewHead = (head.Id == 0);
            
            MyDataContext db = new MyDataContext(Database.ConnectionString);
            int Transaction_Id = head.Id;

            // HEAD record

            if (!lNewHead) // Modify
            {
                var headQuery = db.TransactionHead.Where(p => p.Id == head.Id);
                foreach (var record in headQuery)
                {
                    lSuccess = true;

                    PropertyInfo[] properties = typeof(TransactionHeadEntity).GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        property.SetValue(record, property.GetValue(head));
                    }
                }
            }
            else // Insert
            {
                lSuccess = Database.Add<TransactionHeadEntity>(head);
                Transaction_Id = head.Id;
            }

            if (!lSuccess)
                return false;

            // BODY records
            lSuccess = false;
            // Set transaction ID
            foreach (var rec in body)
            {
                rec.Body.Transaction_Id = Transaction_Id;
            }

            // Delete or modify existing records
            var query = db.TransactionBody.Where(p => p.Transaction_Id == head.Id);
            foreach( var rec in query)
            {
                var newrec = body.Where(p => p.Body.Id == rec.Id).FirstOrDefault();
                if (newrec == null)
                {
                    db.TransactionBody.DeleteOnSubmit(rec);
                }
                else
                {
                    PropertyInfo[] properties = typeof(TransactionBodyEntity).GetProperties();
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
                db.TransactionBody.InsertOnSubmit(rec.Body);
            }

            try
            {
                db.SubmitChanges();
                lSuccess = true;
            }
            catch /*(Exception e)*/
            {
                if (lNewHead)
                    Database.Remove<TransactionHeadEntity>(p => p.Id == Transaction_Id);
            }

            return lSuccess;
        }

        public static bool RemoveTransaction(Expression<Func<TransactionHeadEntity, bool>> condition)
        {
            bool lSuccess = false;
            List<int> DeletedTransactions = new List<int>();
            MyDataContext db = new MyDataContext(Database.ConnectionString);
            // Delete head
            var query = db.TransactionHead.Where(condition);
            foreach(var rec in query)
            {
                DeletedTransactions.Add(rec.Id);
                db.TransactionHead.DeleteOnSubmit(rec);
            }

            // Delete body
            foreach(var rec in db.TransactionBody)
            {
                foreach(var DeletedId in DeletedTransactions)
                {
                    if (DeletedId == rec.Transaction_Id)
                    {
                        db.TransactionBody.DeleteOnSubmit(rec);
                    }
                }
            }

            try
            {
                db.SubmitChanges();
                lSuccess = true;
            }
            catch /*(Exception e)*/ { }

            return lSuccess;
        }
    }
}
