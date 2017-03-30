using BusinessLayer;
using Microsoft.Win32;
using Red_Inventory_Management.Model;
using Red_Inventory_Management.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Red_Inventory_Management.ViewModel
{
    class SetupConnectionViewModel : BindableBase
    {
        public string File
        {
            get
            {
                return DatabaseConnection.File;
            }
            set
            {
                if (value == DatabaseConnection.File) return;
                DatabaseConnection.ChangeDatabaseFile(value);
                RaisePropertyChanged("File");
                RaisePropertyChanged("ConnectionState");
            }
        }
        public string ConnectionState
        {
            get
            {
                return DatabaseConnection.TestConnection() ? "Connected" : "Not connected";
            }
        }

        private ICommand change_database_fileCommand;
        public ICommand Change_Database_FileCommand
        {
            get
            {
                if (change_database_fileCommand == null) change_database_fileCommand = new RelayCommand(new Action<object>(Change_File));
                return change_database_fileCommand;
            }
            set { SetProperty(ref change_database_fileCommand, value); }
        }



        private void Change_File(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ValidateNames = false;
            if (openFileDialog.ShowDialog() == true)
            {
                DatabaseConnection.ChangeDatabaseFile(openFileDialog.FileName);
                RaisePropertyChanged("File");
                RaisePropertyChanged("ConnectionState");
            }
        }
    }
}
