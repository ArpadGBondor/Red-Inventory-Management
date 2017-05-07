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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private PartnerTransactionsDetailsViewModel() { }
        public PartnerTransactionsDetailsViewModel(int partnerId)
            :this()
        {
            _partnerId = partnerId;
            RefreshList(null);
        }

        private int _partnerId;

        private decimal _totalTransactions;
        public decimal TotalTransactions
        {
            get { return _totalTransactions; }
            set { SetProperty(ref _totalTransactions, value); }
        }

        protected override void RefreshList(object parameter)
        {
            log.Debug("Refresh list: Partner transaction summary details");

            List = ManageTransactions.ListHead(_partnerId);
            TotalTransactions = 0;
            foreach (var record in List)
                TotalTransactions += record.ListVariable;
        }
    }
}
