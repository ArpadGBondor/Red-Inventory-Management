using Red_software.Model;
using Red_software.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Red_software.ViewModel
{
    public class SettingsMenuViewModel : BindableBase
    {
        #region Views
        UsersViewModel users = new UsersViewModel();
        #endregion
        #region Change Views
        private BindableBase currentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }
        private ICommand switchViewCommand;
        public ICommand SwitchViewCommand
        {
            get
            {
                if (switchViewCommand == null) switchViewCommand = new RelayCommand(new Action<object>(Navigate));
                return switchViewCommand;
            }
            set { SetProperty(ref switchViewCommand, value); }
        }
        private void Navigate(object parameter)
        {
            string destination = (string)parameter;
            switch (destination)
            {
                case "Users":
                    CurrentViewModel = users;
                    break;
                default:
                    break;
            }
        }
        #endregion



    }
}
