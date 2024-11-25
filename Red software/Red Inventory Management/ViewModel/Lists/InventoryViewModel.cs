﻿using BusinessLayer;
using EntityLayer;
using Red_Inventory_Management.Model;
using Red_Inventory_Management.Views;
using System;
using System.Windows.Input;

namespace Red_Inventory_Management.ViewModel
{
    public class InventoryViewModel : ListModel<TransactionBodyListEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public InventoryViewModel()
        {
            TableName = "Inventory list";
        }
        protected override void RefreshList(object parameter)
        {
            log.Debug("Refresh list: " + TableName);

            List = ManageTransactions.ListInventory();
        }

        private ICommand _detailsCommand;
        public ICommand DetailsCommand
        {
            get
            {
                if (_detailsCommand == null) _detailsCommand = new RelayCommand(new Action<object>(Details), new Predicate<object>(DetailsCanExecute));
                return _detailsCommand;
            }
            set { SetProperty(ref _detailsCommand, value); }
        }

        private bool DetailsCanExecute(object parameter)
        {
            return ItemSelected(parameter);
        }

        private void Details(object parameter)
        {
            log.Debug(TableName + " - details button");

            InventoryDetailsViewModel IDVM = new InventoryDetailsViewModel(SelectedItem.Product.Id);
            ListDetailsWindow LDW = new ListDetailsWindow() { DataContext = IDVM };
            LDW.ShowDialog();
        }
    }
}
