using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using BusinessLayer;
using Red_software.Views;
using Red_software.Notifications;

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
                if (userID == value) return;
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
                if (password == value) return;
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
                if (click_Login == value) return;
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
                NotificationProvider.Alert("Database connection error", "Please set the database connection, before login.");
            }
            else
            {
                try
                {
                    UserLogin.Login(UserID, Password);
                    NotificationProvider.Info(String.Format("Welcome, {0}!", UserID), "You have succesfully logged in.");
                    LoginWindow.Close();
                }
                catch (ArgumentException e)
                {
                    NotificationProvider.Error("Login error", e.Message);
                }
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
                if (click_Setup == value) return;
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
