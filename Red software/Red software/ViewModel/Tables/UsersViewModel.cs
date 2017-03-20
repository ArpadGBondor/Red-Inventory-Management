using Red_software.Model;
using Red_software.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EntityLayer;
using BusinessLayer;
using Red_software.Views;

namespace Red_software.ViewModel
{
    public class UsersViewModel : TableModel<UserEntity>
    {
        protected override void DeleteItem(object parameter)
        {
            string UserID = SelectedItem.Username;
            UserLogin.RemoveUser(UserID);
            NotificationProvider.Info("User deleted", String.Format("Username: {0}", UserID));
            RefreshList(parameter);
        }

        protected override void EditItem(object parameter)
        {
            string UserID = SelectedItem.Username;
            EditUserViewModel EUVM = new EditUserViewModel(UserID);
            EditUserView EUV = new EditUserView() { DataContext = EUVM };
            EUVM.EditWindow = EUV;
            EUV.ShowDialog();
            RefreshList(parameter);
        }

        protected override void NewItem(object parameter)
        {
            NewUserWindow NUW = new NewUserWindow();
            NUW.ShowDialog();
            RefreshList(parameter);
        }

        protected override void RefreshList(object parameter)
        {
            List = UserLogin.ListUsers();
        }
    }
}
