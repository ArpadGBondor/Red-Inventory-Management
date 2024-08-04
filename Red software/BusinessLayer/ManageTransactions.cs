using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using DataLayer;


namespace BusinessLayer
{
    public class ManageTransactions
    {
        public static List<TransactionHeadListEntity> ListHead(bool Incoming)
        {
            var list = TransactionProvider.ListHead(p => p.Incoming == Incoming);
            list.Sort();
            return list;
        }
        public static List<TransactionHeadListEntity> ListHead(int partnerId)
        {
            var list = TransactionProvider.ListHead(p => p.PartnerId == partnerId);
            list.Sort();
            foreach (var record in list)
                record.ListVariable = record.Head.TotalPrice * (record.Head.Incoming ? -1 : 1);
            return list;
        }
        public static List<TransactionBodyListEntity> ListBody(int transactionId)
        {
            return TransactionProvider.ListBody(p => p.TransactionId == transactionId);
        }
        public static bool AddOrModifyTransaction(TransactionHeadEntity head, List<TransactionBodyListEntity> body)
        {
            return TransactionProvider.AddOrModifyTransaction(head, body);
        }
        public static bool RemoveTransaction(TransactionHeadEntity head)
        {
            return TransactionProvider.RemoveTransaction(p => p.Id == head.Id);
        }

        public static List<TransactionBodyListEntity> ListInventory()
        {
            return TransactionProvider.ListInventory(p => true);
        }

        public static List<TransactionHeadListEntity> ListInventoryDetails(int productId)
        {
            var list = TransactionProvider.ListInventoryDetails(productId);
            list.Sort();
            return list;
        }

        public static List<TransactionHeadListEntity> ListPartnerTransactions()
        {
            var list = TransactionProvider.ListPartnerTransactions(p=>true);
            list.Sort();
            return list;
        }

        public static List<TransactionHeadListEntity> ListPartnerTransactionsByDate(DateTime? date = null)
        {
            var list = TransactionProvider.ListPartnerTransactions(p => date == null || p.Date == date);
            list.Sort();
            return list;
        }
    }
}
