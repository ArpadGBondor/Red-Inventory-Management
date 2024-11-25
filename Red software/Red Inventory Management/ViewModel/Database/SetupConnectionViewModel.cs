using BusinessLayer;
using Red_Inventory_Management.Model;
using Red_Inventory_Management.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Red_Inventory_Management.ViewModel
{
    class SetupConnectionViewModel : BindableBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Window SetupWindow { get; set; }

        private static string _baseDir
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

        private string _directory;
        public string Directory
        {
            get
            {
                if (_directory == null)
                {
                    _directory = (string.IsNullOrWhiteSpace(DatabaseConnection.Directory) ? _baseDir : DatabaseConnection.Directory);
                    CollectDbNames(_directory);
                }
                return _directory;
            }
            set
            {
                if (_directory != value)
                    CollectDbNames(value);
                SetProperty(ref _directory, value);
            }
        }

        private string _dbname;
        public string DbName
        {
            get
            {
                if (_dbname == null) _dbname = (string.IsNullOrWhiteSpace(DatabaseConnection.DbName) ? "Database" : DatabaseConnection.DbName);
                return _dbname;
            }
            set { SetProperty(ref _dbname, value); }
        }

        private List<string> _dbnamelist;
        public List<string> DbNameList
        {
            get { return _dbnamelist; }
            set { SetProperty(ref _dbnamelist, value); }
        }

        private void CollectDbNames(string dir)
        {
            List<string> list = new List<string>();
            var filenames = System.IO.Directory.GetFiles(dir, "*.mdf");
            foreach (var name in filenames)
            {
                list.Add(Path.GetFileNameWithoutExtension(name));
            }
            DbNameList = list;
        }

        private ICommand _connectDatabaseCommand;
        public ICommand ConnectDatabaseCommand
        {
            get
            {
                if (_connectDatabaseCommand == null) _connectDatabaseCommand = new RelayCommand(new Action<object>(ConnectDatabase), new Predicate<object>(CanConnectDatabase));
                return _connectDatabaseCommand;
            }
            set { SetProperty(ref _connectDatabaseCommand, value); }
        }

        private void ConnectDatabase(object parameter)
        {
            log.Debug("Connect to database");

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

        private ICommand _createDatabaseCommand;
        public ICommand CreateDatabaseCommand
        {
            get
            {
                if (_createDatabaseCommand == null) _createDatabaseCommand = new RelayCommand(new Action<object>(CreateDatabase), new Predicate<object>(CanCreateDatabase));
                return _createDatabaseCommand;
            }
            set { SetProperty(ref _createDatabaseCommand, value); }
        }

        private void CreateDatabase(object parameter)
        {
            log.Debug("Create database");

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

        private ICommand _selectDirectoryCommand;
        public ICommand SelectDirectoryCommand
        {
            get
            {
                if (_selectDirectoryCommand == null) _selectDirectoryCommand = new RelayCommand(new Action<object>(SelectDirectory));
                return _selectDirectoryCommand;
            }
            set { SetProperty(ref _selectDirectoryCommand, value); }
        }

        private FolderBrowserDialog _FBD = new FolderBrowserDialog();

        private void SelectDirectory(object parameter)
        {
            _FBD.SelectedPath = Directory;
            if (_FBD.ShowDialog() == DialogResult.OK)
            {
                Directory = _FBD.SelectedPath;
                if (!Directory.EndsWith("\\"))
                    Directory += "\\";
            }
        }
    }
}
