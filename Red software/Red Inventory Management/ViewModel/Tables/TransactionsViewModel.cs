using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using BusinessLayer;
using Red_Inventory_Management.Model;
using Red_Inventory_Management.Notifications;
using Red_Inventory_Management.Views;

namespace Red_Inventory_Management.ViewModel
{
    public class TransactionsViewModel : TableModel<TransactionHeadListEntity>
    {
        private TransactionsViewModel() { }
        public TransactionsViewModel(bool _Incoming)
        {
            incoming = _Incoming;
            ItemName = "transaction";
            TableName = (Incoming ? "Incoming" : "Outgoing") + " transactions";
        }
        private bool incoming;
        public bool Incoming { get { return incoming; } }

        protected override void DeleteItem(object parameter)
        {
            string date = SelectedItem.Date.ToString("d");
            string PartnerName = SelectedItem.Partner.Name;
            int id = SelectedItem.Head.Id;
            if (ManageTransactions.RemoveTransaction(SelectedItem.Head))
            {
                RefreshList(parameter);
                NotificationProvider.Info("Transaction deleted", string.Format("Id: {0}\nDate: {1}\nPartner name: {2}",id,date, PartnerName));
            }
            else
            {
                NotificationProvider.Error("Delete transaction error", "Unknown reason.");
            }
        }

        protected override void EditItem(object parameter)
        {
            TransactionHeadListEntity Item = new TransactionHeadListEntity();
            EntityCloner.CloneProperties<TransactionHeadListEntity>(Item, SelectedItem);
            EditTransactionViewModel ETVM = new EditTransactionViewModel(Item, false,ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = ETVM };
            EIV.ShowDialog();
            if (ETVM.SaveEdit)
            {
                Item = ETVM.Item;
                NotificationProvider.Info("Transaction saved", string.Format("Id: {0}\nDate: {1}\nPartner name: {2}", Item.Head.Id, Item.Date.ToString("d"), Item.Partner.Name));
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
            EditTransactionViewModel ETVM = new EditTransactionViewModel(Item, true, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = ETVM };
            EIV.ShowDialog();
            if (ETVM.SaveEdit)
            {
                Item = ETVM.Item;
                NotificationProvider.Info("Transaction added", string.Format("Id: {0}\nDate: {1}\nPartner name: {2}", Item.Head.Id, Item.Date.ToString("d"), Item.Partner.Name));
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
