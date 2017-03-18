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
    public class UsersViewModel : BindableBase
    {
        private List<UserEntity> userlist;
        public List<UserEntity> UserList
        {
            get { return userlist; }
            set { SetProperty(ref userlist, value); }
        }

        private UserEntity selectedItem;
        public UserEntity SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); }
        }

        private ICommand newUserCommand;
        public ICommand NewUserCommand
        {
            get
            {
                if (newUserCommand == null) newUserCommand = new RelayCommand(new Action<object>(NewUser));
                return newUserCommand;
            }
            set { SetProperty(ref newUserCommand, value); }
        }

        private void NewUser(object parameter)
        {
            NewUserWindow NUW = new NewUserWindow();
            NUW.ShowDialog();
            Reload(null);
        }

        private ICommand deleteUserCommand;
        public ICommand DeleteUserCommand
        {
            get
            {
                if (deleteUserCommand == null) deleteUserCommand = new RelayCommand(new Action<object>(DeleteUser), new Predicate<object>(CanDeleteUser));
                return deleteUserCommand;
            }
            set { SetProperty(ref deleteUserCommand, value); }
        }

        private void DeleteUser(object parameter)
        {
            UserLogin.RemoveUser(SelectedItem.Username);
            Reload(null);
        }

        private bool CanDeleteUser(object parameter)
        {
            return (SelectedItem != null);
        }

        private ICommand refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null) refreshCommand = new RelayCommand(new Action<object>(Reload));
                return refreshCommand;
            }
            set { SetProperty(ref refreshCommand, value); }
        } 
        private void Reload(object parameter)
        {
            UserList = UserLogin.ListUsers();
        }
    }
}
