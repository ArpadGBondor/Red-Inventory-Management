using System.Collections.Generic;
using EntityLayer;
using System.Data.Linq;
using System.Linq;
using System;
using System.Linq.Expressions;
using System.Data.Linq.Mapping;

namespace DataLayer
{
    public class TransactionProvider
    {
        static TransactionProvider()
        {
            Database.InitializeTable(typeof(TransactionHeadEntity));
            Database.InitializeTable(typeof(TransactionBodyEntity));
            Database.InitializeTable(typeof(ProductEntity));
            Database.InitializeTable(typeof(PartnerEntity));
        }

        public static List<TransactionHeadListEntity> HeaderList(Expression<Func<TransactionHeadListEntity,bool>> condition)
        {
            List<TransactionHeadListEntity> list = new List<TransactionHeadListEntity>();
            MyDataContext db = new MyDataContext(Database.get_connectionString);
            var query = (from h in db.TransactionHead
                            join p in db.Partners on h.Partner_Id equals p.Id into PartJoin
                            from subp in PartJoin.DefaultIfEmpty()
                            select new TransactionHeadListEntity(h, subp));
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

        public static bool AddModifyTransaction(TransactionHeadEntity head, List<TransactionBodyListEntity> body)
        {
            bool lSuccess = false;
            

            MyDataContext db = new MyDataContext(Database.get_connectionString);
               

            return lSuccess;
        }

        //public static bool AddHead(TransactionHeadEntity head)
        //{
        //    return Database.Add<TransactionHeadEntity>(head);
        //}
        //public static bool AddBody(TransactionBodyEntity head)
        //{
        //    return Database.Add<TransactionBodyEntity>(head);
        //}
        //public static bool ModifyHead(TransactionHeadEntity head)
        //{
        //    return Database.Modify<TransactionHeadEntity>(head, p => p.Id == head.Id);
        //}
        //public static bool ModifyBody(TransactionBodyEntity head)
        //{
        //    return Database.Modify<TransactionBodyEntity>(head, p => p.Id == head.Id);
        //}


    }
}
