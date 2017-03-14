using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using BusinessLayer;
using Red_software.Notifications;

namespace Red_software.ViewModel
{
    class NewUserViewModel : INotifyPropertyChanged
    {
        private string userID;
        private string password;
        private string confirm;
        public Window NewUserWindow { get; set; }
        public string UserID
        {
            get
            {
                if (userID == null)
                    userID = "";
                return userID;
            }
            set
            {
                if (userID == value) return;
                userID = value;
                RaisePropertyChanged("UserID");
            }
        }
        public string Password
        {
            get
            {
                if (password == null)
                    password = "";
                return password;
            }
            set
            {
                if (password == value) return;
                password = value;
                RaisePropertyChanged("Password");
            }
        }
        public string Confirm
        {
            get
            {
                if (confirm == null)
                    confirm = "";
                return confirm;
            }
            set
            {
                if (confirm == value) return;
                confirm = value;
                RaisePropertyChanged("Confirm");
            }
        }

        private ICommand click_AddUser;

        public ICommand Click_AddUser
        {
            get
            {
                if (click_AddUser == null)
                    click_AddUser = new RelayCommand(new Action<object>(AddUser));
                return click_AddUser;
            }
        }
        private void AddUser(object parameter)
        {
            if (UserID == "" || Password == "" || Confirm == "")
            {
                NotificationProvider.Error("New user error", "Please fill the Username and Password fieleds.");
            }
            else if(Password != Confirm)
            {
                NotificationProvider.Error("New user error", "Password does not match the confirm password.");
            }
            else if (UserLogin.IsValidUserID(UserID))
            {
                NotificationProvider.Error("New user error", "Username already exist.");
            }
            else
            {
                UserLogin.AddUser(UserID, Password);
                NotificationProvider.Info("New user added", String.Format("Username: {0}",UserID));
                NewUserWindow.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

    }
}
