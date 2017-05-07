using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using BusinessLayer;
using Red_Inventory_Management.Notifications;
using Red_Inventory_Management.Model;

namespace Red_Inventory_Management.ViewModel
{
    class NewUserViewModel : BindableBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _userID;
        private string _password;
        private string _confirm;
        public Window NewUserWindow { get; set; }
        public string UserID
        {
            get
            {
                if (_userID == null) _userID = "";
                return _userID;
            }
            set { SetProperty(ref _userID, value); }
        }
        public string Password
        {
            get
            {
                if (_password == null) _password = "";
                return _password;
            }
            set { SetProperty(ref _password, value); }
        }
        public string Confirm
        {
            get
            {
                if (_confirm == null) _confirm = "";
                return _confirm;
            }
            set { SetProperty(ref _confirm, value); }
        }

        private ICommand _click_AddUserCommand;

        public ICommand Click_AddUserCommand
        {
            get
            {
                if (_click_AddUserCommand == null) _click_AddUserCommand = new RelayCommand(new Action<object>(AddUser));
                return _click_AddUserCommand;
            }
            set { SetProperty(ref _click_AddUserCommand, value); }
        }
        private void AddUser(object parameter)
        {
            log.Debug("Add user button");

            if (Password != Confirm)
            {
                NotificationProvider.Error("New user error", "Password does not match the confirm password.");
            }
            else
            {
                try
                {
                    if (UserLogin.AddUser(UserID, Password))
                    {
                        NotificationProvider.Info("New user added", String.Format("Username: {0}", UserID));
                        NewUserWindow.Close();
                    }
                    else
                    {
                        NotificationProvider.Error("New user error", "Username already exist.");
                    }
                }
                catch
                {
                    NotificationProvider.Error("New user error", "Please fill the Username and Password fieleds.");
                }
            }
        }
    }
}
