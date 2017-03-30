using EntityLayer;
using BusinessLayer;
using Red_Inventory_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Red_Inventory_Management.ViewModel
{
    class PartnerTransactionsDetailsViewModel : ListModel<TransactionHeadListEntity>
    {
        private PartnerTransactionsDetailsViewModel() { }
        public PartnerTransactionsDetailsViewModel(int _Partner_Id)
            :this()
        {
            Partner_Id = _Partner_Id;
            RefreshList(null);
        }

        private int Partner_Id;

        private decimal totalTransactions;
        public decimal TotalTransactions
        {
            get { return totalTransactions; }
            set { SetProperty(ref totalTransactions, value); }
        }

        protected override void RefreshList(object parameter)
        {
            List = ManageTransactions.ListHead(Partner_Id);
            TotalTransactions = 0;
            foreach (var record in List)
                TotalTransactions += record.ListVariable;
        }
    }
}
