using BusinessLayer;
using Microsoft.Win32;
using Red_Inventory_Management.Model;
using Red_Inventory_Management.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows;

namespace Red_Inventory_Management.ViewModel
{
    class SetupConnectionViewModel : BindableBase
    {
        public Window SetupWindow { get; set; }

        private static string BaseDir
        {
            get
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                if (!baseDir.EndsWith("\\"))
                    baseDir += "\\";
                return baseDir;
            }
        }

        public string ConnectionState { get { return DatabaseConnection.TestConnection() ? "Connected" : "Not connected"; } }

        public string ConnectedFile
        {
            get
            {
                string file = DatabaseConnection.Directory + DatabaseConnection.DbName;
                if (!string.IsNullOrWhiteSpace(file))
                    file += ".mdf";
                return file;
            }
        }

        private string directory;
        public string Directory
        {
            get
            {
                if (directory == null)
                {
                    directory = (string.IsNullOrWhiteSpace(DatabaseConnection.Directory) ? BaseDir : DatabaseConnection.Directory);
                    CollectDbNames(directory);
                }
                return directory;
            }
            set
            {
                if (directory != value)
                    CollectDbNames(value);
                SetProperty(ref directory, value);
            }
        }

        private string dbname;
        public string DbName
        {
            get
            {
                if (dbname == null) dbname = (string.IsNullOrWhiteSpace(DatabaseConnection.DbName) ? "Database" : DatabaseConnection.DbName) ;
                return dbname;
            }
            set { SetProperty(ref dbname, value); }
        }

        private List<string> dbnamelist;
        public List<string> DbNameList
        {
            get { return dbnamelist; }
            set { SetProperty(ref dbnamelist, value); }
        }

        private void CollectDbNames(string dir)
        {
            List<string> list = new List<string>();
            var filenames = System.IO.Directory.GetFiles(dir, "*.mdf");
            foreach(var name in filenames)
            {
                list.Add(Path.GetFileNameWithoutExtension(name));
            }
            DbNameList = list;
        }

        private ICommand connectDatabaseCommand;
        public ICommand ConnectDatabaseCommand
        {
            get
            {
                if (connectDatabaseCommand == null) connectDatabaseCommand = new RelayCommand(new Action<object>(ConnectDatabase), new Predicate<object>(CanConnectDatabase));
                return connectDatabaseCommand;
            }
            set { SetProperty(ref connectDatabaseCommand, value); }
        }

        private void ConnectDatabase(object parameter)
        {
            if (DatabaseConnection.ChangeDatabase(Directory, DbName))
            {
                NotificationProvider.Info("Connected to:", ConnectedFile);
                SetupWindow?.Close();
            }
            else
                NotificationProvider.Error("Connection error", "Database connection failed.");
            RaisePropertyChanged("ConnectionState");
            RaisePropertyChanged("ConnectedFile");
        }

        private bool CanConnectDatabase(object parameter)
        {
            return File.Exists(Directory + DbName + ".mdf");
        }

        private ICommand createDatabaseCommand;
        public ICommand CreateDatabaseCommand
        {
            get
            {
                if (createDatabaseCommand == null) createDatabaseCommand = new RelayCommand(new Action<object>(CreateDatabase), new Predicate<object>(CanCreateDatabase));
                return createDatabaseCommand;
            }
            set { SetProperty(ref createDatabaseCommand, value); }
        }

        private void CreateDatabase(object parameter)
        {
            if (DatabaseConnection.CreateDatabase(Directory, DbName))
            {
                RaisePropertyChanged("ConnectionState");
                RaisePropertyChanged("ConnectedFile");
                CollectDbNames(Directory);
                NotificationProvider.Info("Database created", "New database: " + DbName);
                NotificationProvider.Info("Connected to:", ConnectedFile);
                SetupWindow?.Close();
            }
            else
                NotificationProvider.Error("New database error", "Database creation failed.");
        }

        private bool CanCreateDatabase(object parameter)
        {
            return !File.Exists(Directory + DbName + ".mdf");
        }

        private ICommand selectDirectoryCommand;
        public ICommand SelectDirectoryCommand
        {
            get
            {
                if (selectDirectoryCommand == null) selectDirectoryCommand = new RelayCommand(new Action<object>(SelectDirectory));
                return selectDirectoryCommand;
            }
            set { SetProperty(ref selectDirectoryCommand, value); }
        }

        private FolderBrowserDialog FBD = new FolderBrowserDialog();

        private void SelectDirectory(object parameter)
        {
            FBD.SelectedPath = Directory;
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                Directory = FBD.SelectedPath;
                if (!Directory.EndsWith("\\"))
                    Directory += "\\";
            }
        }
    }
}
