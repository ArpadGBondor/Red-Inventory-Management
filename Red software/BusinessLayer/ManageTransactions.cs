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
            return TransactionProvider.ListHead(p => p.Incoming == Incoming);
        }
        public static List<TransactionBodyListEntity> ListBody(int transactionId)
        {
            return TransactionProvider.ListBody(p => p.Transaction_Id == transactionId);
        }
        public static bool AddOrModifyTransaction(TransactionHeadEntity head, List<TransactionBodyListEntity> body)
        {
            return TransactionProvider.AddOrModifyTransaction(head, body);
        }
        public static bool RemoveTransaction(TransactionHeadEntity head)
        {
            return TransactionProvider.RemoveTransaction(p => p.Id == head.Id);
        }
    }
}
