using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using BusinessLayer;
using Red_software.Model;
using Red_software.Notifications;
using Red_software.Views;

namespace Red_software.ViewModel
{
    public class TransactionsViewModel : TableModel<TransactionHeadListEntity>
    {
        private TransactionsViewModel() { }
        public TransactionsViewModel(bool _Incoming) { incoming = _Incoming; }
        private bool incoming;
        public bool Incoming { get { return incoming; } }

        protected override void DeleteItem(object parameter)
        {
            if (ManageTransactions.RemoveTransaction(SelectedItem.Head))
                RefreshList(parameter);
            else
            {
                NotificationProvider.Error("Delete transaction error", "Unknown reason.");
            }
        }

        protected override void EditItem(object parameter)
        {
            TransactionHeadListEntity Item = new TransactionHeadListEntity();
            EntityCloner.CloneProperties<TransactionHeadListEntity>(Item, SelectedItem);
            EditTransactionViewModel ETVM = new EditTransactionViewModel(Item, false);
            EditItemView EIV = new EditItemView() { DataContext = ETVM };
            EIV.ShowDialog();
            if (ETVM.SaveEdit)
            {
                Item = ETVM.Item;
                RefreshList(parameter);
                foreach (var t in List)
                    if (Item.Head.Id == t.Head.Id)
                        SelectedItem = t;
            }
        }

        protected override void NewItem(object parameter)
        {
            TransactionHeadListEntity Item = new TransactionHeadListEntity();
            Item.Head = new TransactionHeadEntity();
            Item.Head.Incoming = this.Incoming;
            Item.Date = DateTime.Now;
            EditTransactionViewModel ETVM = new EditTransactionViewModel(Item, false);
            EditItemView EIV = new EditItemView() { DataContext = ETVM };
            EIV.ShowDialog();
            if (ETVM.SaveEdit)
            {
                Item = ETVM.Item;
                RefreshList(parameter);
                foreach (var t in List)
                    if (Item.Head.Id == t.Head.Id)
                        SelectedItem = t;
            }
        }

        protected override void RefreshList(object parameter)
        {
            List = ManageTransactions.ListHead(Incoming);
        }
    }
}
