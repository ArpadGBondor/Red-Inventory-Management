using BusinessLayer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Red_software.ViewModel
{
    class SetupConnectionViewModel : INotifyPropertyChanged
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

        private ICommand change_database_file;
        public ICommand Change_Database_File
        {
            get
            {
                if (change_database_file == null)
                    change_database_file = new RelayCommand(new Action<object>(Change_File));
                return change_database_file;
            }
            set
            {
                if (change_database_file == value) return;
                change_database_file = value;
                RaisePropertyChanged("Change_Database_File");
            }
        }

        private void Change_File(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                DatabaseConnection.ChangeDatabaseFile(openFileDialog.FileName);
                RaisePropertyChanged("File");
                RaisePropertyChanged("ConnectionState");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
