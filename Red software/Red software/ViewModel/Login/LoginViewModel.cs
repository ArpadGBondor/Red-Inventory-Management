using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using BusinessLayer;
using Red_Inventory_Management.Views;
using Red_Inventory_Management.Model;
using Red_Inventory_Management.Notifications;

namespace Red_Inventory_Management.ViewModel
{
    class LoginViewModel : BindableBase
    {
        private string userID;
        private string password;
        public Window LoginWindow { get; set; }
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

        private ICommand click_LoginCommand;
        public ICommand Click_LoginCommand
        {
            get
            {
                if (click_LoginCommand == null) click_LoginCommand = new RelayCommand(new Action<object>(Login));
                return click_LoginCommand;
            }
            set { SetProperty(ref click_LoginCommand, value); }
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

        private ICommand click_SetupCommand;
        public ICommand Click_SetupCommand
        {
            get
            {
                if (click_SetupCommand == null) click_SetupCommand = new RelayCommand(new Action<object>(Setup));
                return click_SetupCommand;
            }
            set { SetProperty(ref click_SetupCommand, value); }
        }

        private void Setup(object parameter)
        {
            Views.SetupConnectionWindow setupWindow = new SetupConnectionWindow();
            setupWindow.ShowDialog();
        }
    }
}
