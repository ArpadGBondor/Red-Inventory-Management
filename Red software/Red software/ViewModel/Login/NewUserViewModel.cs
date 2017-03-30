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
        private string userID;
        private string password;
        private string confirm;
        public Window NewUserWindow { get; set; }
        public string UserID
        {
            get
            {
                if (userID == null) userID = "";
                return userID;
            }
            set { SetProperty(ref userID, value); }
        }
        public string Password
        {
            get
            {
                if (password == null) password = "";
                return password;
            }
            set { SetProperty(ref password, value); }
        }
        public string Confirm
        {
            get
            {
                if (confirm == null) confirm = "";
                return confirm;
            }
            set { SetProperty(ref confirm, value); }
        }

        private ICommand click_AddUserCommand;

        public ICommand Click_AddUserCommand
        {
            get
            {
                if (click_AddUserCommand == null) click_AddUserCommand = new RelayCommand(new Action<object>(AddUser));
                return click_AddUserCommand;
            }
            set { SetProperty(ref click_AddUserCommand, value); }
        }
        private void AddUser(object parameter)
        {
            if(Password != Confirm)
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
