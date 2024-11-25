using BusinessLayer;
using Red_Inventory_Management.Model;
using Red_Inventory_Management.Notifications;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Red_Inventory_Management.ViewModel
{
    class EditUserViewModel : BindableBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EditUserViewModel() { }
        public EditUserViewModel(string oldUserID) { this._oldUserID = oldUserID; }

        private Window _editWindow;
        public Window EditWindow
        {
            get { return _editWindow; }
            set { SetProperty(ref _editWindow, value); }
        }

        private string _oldUserID;
        private string _newUserID;
        private string _password;
        private string _confirm;
        public string UserID
        {
            get
            {
                if (_newUserID == null) _newUserID = _oldUserID;
                return _newUserID;
            }
            set { SetProperty(ref _newUserID, value); }
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

        private ICommand _modifyUserCommand;
        public ICommand ModifyUserCommand
        {
            get
            {
                if (_modifyUserCommand == null) _modifyUserCommand = new RelayCommand(new Action<object>(ModifyUser));
                return _modifyUserCommand;
            }
            set { SetProperty(ref _modifyUserCommand, value); }
        }
        private void ModifyUser(object parameter)
        {
            log.Debug("Modify user button");

            PasswordBox pwBox = (PasswordBox)parameter;
            string OldPassword = pwBox.Password;

            if (string.IsNullOrWhiteSpace(_oldUserID) ||
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
                    if (UserLogin.ModifyUser(_oldUserID, OldPassword, UserID, Password, Confirm))
                    {
                        NotificationProvider.Info(String.Format("User modified: {0}", _oldUserID), String.Format("New username: {0}", UserID));
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
