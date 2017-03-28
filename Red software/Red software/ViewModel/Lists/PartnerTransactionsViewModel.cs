using BusinessLayer;
using EntityLayer;
using Red_software.Model;
using Red_software.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Red_software.ViewModel
{
    public class PartnerTransactionsViewModel : ListModel<TransactionHeadListEntity>
    {
        public PartnerTransactionsViewModel()
        {
            TableName = "Partner transaction summary";
        }

        private decimal totalTransactions;
        public decimal TotalTransactions
        {
            get { return totalTransactions; }
            set { SetProperty(ref totalTransactions, value); }
        }
        protected override void RefreshList(object parameter)
        {
            List = ManageTransactions.ListPartnerTransactions();
            TotalTransactions = 0;
            foreach (var record in List)
                TotalTransactions += record.ListVariable;
        }

        private ICommand detailsCommand;
        public ICommand DetailsCommand
        {
            get
            {
                if (detailsCommand == null) detailsCommand = new RelayCommand(new Action<object>(Details), new Predicate<object>(DetailsCanExecute));
                return detailsCommand;
            }
            set { SetProperty(ref detailsCommand, value); }
        }

        private bool DetailsCanExecute(object parameter)
        {
            return ItemSelected(parameter);
        }

        private void Details(object parameter)
        {
            PartnerTransactionsDetailsViewModel PTDVM = new PartnerTransactionsDetailsViewModel(SelectedItem.Partner.Id);
            ListDetailsWindow LDW = new ListDetailsWindow() { DataContext = PTDVM };
            LDW.ShowDialog();
        }
    }
}
