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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TransactionsViewModel() { }
        public TransactionsViewModel(bool incoming)
        {
            _incoming = incoming;
            ItemName = "transaction";
            TableName = (Incoming ? "Incoming" : "Outgoing") + " transactions";
        }
        private bool _incoming;
        public bool Incoming { get { return _incoming; } }

        protected override void DeleteItem(object parameter)
        {
            log.Debug("Delete " + ItemName + " button");

            string date = SelectedItem.Head.Date.ToString("d");
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
            log.Debug("Edit " + ItemName + " button");

            TransactionHeadListEntity Item = new TransactionHeadListEntity();
            EntityCloner.CloneProperties<TransactionHeadListEntity>(SelectedItem, Item);
            EditTransactionViewModel ETVM = new EditTransactionViewModel(Item, false,ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = ETVM };
            EIV.ShowDialog();
            if (ETVM.SaveEdit)
            {
                Item = ETVM.Item;
                NotificationProvider.Info("Transaction saved", string.Format("Id: {0}\nDate: {1}\nPartner name: {2}", Item.Head.Id, Item.Head.Date.ToString("d"), Item.Partner.Name));
                RefreshList(parameter);
                foreach (var t in List)
                    if (Item.Head.Id == t.Head.Id)
                        SelectedItem = t;
            }
        }

        protected override void NewItem(object parameter)
        {
            log.Debug("New " + ItemName + " button");

            TransactionHeadListEntity Item = new TransactionHeadListEntity();
            Item.Head = new TransactionHeadEntity();
            Item.Head.Incoming = this.Incoming;
            Item.Head.Date = DateTime.Now.Date;
            EditTransactionViewModel ETVM = new EditTransactionViewModel(Item, true, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = ETVM };
            EIV.ShowDialog();
            if (ETVM.SaveEdit)
            {
                Item = ETVM.Item;
                NotificationProvider.Info("Transaction added", string.Format("Id: {0}\nDate: {1}\nPartner name: {2}", Item.Head.Id, Item.Head.Date.ToString("d"), Item.Partner.Name));
                RefreshList(parameter);
                foreach (var t in List)
                    if (Item.Head.Id == t.Head.Id)
                        SelectedItem = t;
            }
        }

        protected override void RefreshList(object parameter)
        {
            log.Debug("Refresh " + ItemName + " list");

            List = ManageTransactions.ListHead(Incoming);
        }
    }
}
