using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using Red_software.Model;
using System.Windows.Input;
using System.Windows.Controls;
using BusinessLayer;
using Red_software.Notifications;
using System.Windows;

namespace Red_software.ViewModel
{
    class EditUserViewModel : BindableBase
    {

        private EditUserViewModel() { }
        public EditUserViewModel(string _OldUserID) { oldUserID = _OldUserID; }

        private Window editWindow;
        public Window EditWindow
        {
            get { return editWindow; }
            set { SetProperty(ref editWindow, value); }
        }

        private string oldUserID;
        private string newUserID;
        private string password;
        private string confirm;
        public string UserID
        {
            get
            {
                if (newUserID == null) newUserID = oldUserID;
                return newUserID;
            }
            set { SetProperty(ref newUserID, value); }
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

        private ICommand modifyUserCommand;

        public ICommand ModifyUserCommand
        {
            get
            {
                if (modifyUserCommand == null) modifyUserCommand = new RelayCommand(new Action<object>(ModifyUser));
                return modifyUserCommand;
            }
            set { SetProperty(ref modifyUserCommand, value); }
        }
        private void ModifyUser(object parameter)
        {
            PasswordBox pwBox = (PasswordBox)parameter;
            string OldPassword = pwBox.Password;

            if (string.IsNullOrWhiteSpace(oldUserID) ||
                string.IsNullOrWhiteSpace(OldPassword) ||
                string.IsNullOrWhiteSpace(UserID) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Confirm))
            {
                NotificationProvider.Error("Edit user error", "Please fill the Username and Password fieleds.");
            }
            else
            {
                try
                {
                    if (UserLogin.ModifyUser(oldUserID, OldPassword, UserID, Password, Confirm))
                    {
                        NotificationProvider.Info(String.Format("User modified: {0}", oldUserID), String.Format("New username: {0}", UserID));
                        EditWindow?.Close();
                    }
                    else
                    {
                        NotificationProvider.Error("Edit user error", "Database error");
                    }
                }
                catch (ArgumentException e)
                {
                    switch (e.ParamName)
                    {
                        case "oldUserID":
                            NotificationProvider.Error("Edit user error", "The original username is missing from the database.");
                            break;
                        case "oldPassword":
                            NotificationProvider.Error("Edit user error", "The old password is wrong.");
                            break;
                        case "newUserId":
                            NotificationProvider.Error("Edit user error", "The new username already exist.");
                            break;
                        case "password":
                            NotificationProvider.Error("Edit user error", "Please fill the password field.");
                            break;
                        case "confirm":
                            NotificationProvider.Error("Edit user error", "Password does not match the confirm password.");
                            break;
                        default:
                            NotificationProvider.Error("Edit user error", "UserLogin error");
                            break;
                    }
                }

            }
        }
    }
}
