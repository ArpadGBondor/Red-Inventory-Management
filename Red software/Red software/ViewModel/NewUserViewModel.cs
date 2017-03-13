using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using BusinessLayer;

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
                return userID;
            }
            set
            {
                userID = value;
                RaisePropertyChanged("UserID");
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }
        public string Confirm
        {
            get
            {
                return confirm;
            }
            set
            {
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
                MessageBox.Show("Please fill the Username and Password fieleds!");
            }
            else if(Password != Confirm)
            {
                MessageBox.Show("Password does not match the confirm password");
            }
            else if (UserLogin.IsValidUserID(UserID))
            {
                MessageBox.Show("Username already exist.");
            }
            else
            {
                UserLogin.AddUser(UserID, Password);
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
