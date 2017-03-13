using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using BusinessLayer;
using Red_software.Views;

namespace Red_software.ViewModel
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public LoginViewModel()
        {
            UserID = "";
            Password = "";
        }
        private string userID;
        private string password;
        public Window LoginWindow { get; set; }
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

        private ICommand click_Login;
        public ICommand Click_Login
        {
            get
            {
                if (click_Login == null)
                    click_Login = new RelayCommand(new Action<object>(Login));
                return click_Login;
            }
            set
            {
                click_Login = value;
                RaisePropertyChanged("Click_Login");
            }
        }
        
        private void Login(object parameter)
        {
            PasswordBox pwBox = (PasswordBox)parameter;
            Password = pwBox.Password;
            if (!DatabaseConnection.TestConnection())
            {
                MessageBox.Show("Database connection error.");
            }
            else if (UserID == "" || !UserLogin.IsValidUserID(UserID))
            {
                MessageBox.Show("Wrong username.");
            }
            else if (Password == "" || !UserLogin.IsValidPassword(UserID,Password))
            {
                MessageBox.Show("Wrong password.");
            }
            else
            {
                UserLogin.UserID = UserID;
                LoginWindow.Close();
            }
        }

        private ICommand click_Setup;
        public ICommand Click_Setup
        {
            get
            {
                if (click_Setup == null)
                    click_Setup = new RelayCommand(new Action<object>(Setup));
                return click_Setup;
            }
            set
            {
                click_Setup = value;
                RaisePropertyChanged("Click_Setup");
            }
        }

        private void Setup(object parameter)
        {
            Views.SetupConnectionWindow setupWindow = new SetupConnectionWindow();
            setupWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
