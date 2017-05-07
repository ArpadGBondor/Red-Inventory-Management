using Red_Inventory_Management.Model;
using Red_Inventory_Management.Notifications;
using System;
using EntityLayer;
using BusinessLayer;
using Red_Inventory_Management.Views;

namespace Red_Inventory_Management.ViewModel
{
    public class UsersViewModel : TableModel<UserEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UsersViewModel()
        {
            ItemName = "user";
            TableName = "Users";
        }
        protected override void DeleteItem(object parameter)
        {
            log.Debug("Delete " + ItemName + " button");

            string UserID = SelectedItem.Username;
            UserLogin.RemoveUser(UserID);
            NotificationProvider.Info("User deleted", String.Format("Username: {0}", UserID));
            RefreshList(parameter);
        }

        protected override void EditItem(object parameter)
        {
            log.Debug("Edit " + ItemName + " button");

            string UserID = SelectedItem.Username;
            EditUserViewModel EUVM = new EditUserViewModel(UserID);
            EditUserWindow EUV = new EditUserWindow() { DataContext = EUVM };
            EUVM.EditWindow = EUV;
            EUV.ShowDialog();
            RefreshList(parameter);
        }

        protected override void NewItem(object parameter)
        {
            log.Debug("New " + ItemName + " button");

            NewUserWindow NUW = new NewUserWindow();
            NUW.ShowDialog();
            RefreshList(parameter);
        }

        protected override void RefreshList(object parameter)
        {
            log.Debug("Refresh " + ItemName + " list");

            List = UserLogin.ListUsers();
        }
    }
}
