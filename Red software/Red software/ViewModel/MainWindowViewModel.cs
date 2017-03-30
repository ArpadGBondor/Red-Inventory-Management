using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Red_Inventory_Management.Model;
using System.Windows;
using System.Windows.Input;
using BusinessLayer;
using Red_Inventory_Management.Views;
using Red_Inventory_Management.Notifications;

namespace Red_Inventory_Management.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        #region Constructors
        public MainWindowViewModel(Window _mainwindow)
        {
            mainWindow = _mainwindow;
            // Set database connection
            if (!DatabaseConnection.TestConnection())
            {
                SetupConnectionWindow SCW = new SetupConnectionWindow();
                SCW.ShowDialog();
                if (!DatabaseConnection.TestConnection())
                    CloseWindow();
            }

            // New user
            if (UserLogin.IsEmptyUserDatabase())
            {
                NotificationProvider.Info("Welcome First User!", "Please, set a username and a password.");
                NewUserWindow NUW = new NewUserWindow();
                NUW.ShowDialog();
                if (UserLogin.IsEmptyUserDatabase())
                    CloseWindow();
            }

            // Login
            LoginWindow LW = new LoginWindow();
            LW.ShowDialog();
            if (UserLogin.LoginedUser == "") // Not logged in
                CloseWindow();
        }
        #endregion
        #region Menus and Views
        private string[] mainMenu = new string[] { "Tables", "Transactions", "Lists", "Settings" };
        private string[] tablesMenu = new string[] { "Products", "Product categories", "Partners" };
        private ProductsViewModel products = new ProductsViewModel();
        private ProductCategoriesViewModel productCategories = new ProductCategoriesViewModel();
        private PartnersViewModel partners = new PartnersViewModel();
        private string[] transactionsMenu = new string[] { "Incoming", "Outgoing" };
        private TransactionsViewModel incomingTransactions = new TransactionsViewModel(true);
        private TransactionsViewModel outgoingTransactions = new TransactionsViewModel(false);
        private string[] listsMenu = new string[] { "Inventory", "Partner transactions" };
        private InventoryViewModel inventory = new InventoryViewModel();
        private PartnerTransactionsViewModel partnerTransactions = new PartnerTransactionsViewModel();
        private string[] settingsMenu = new string[] { "Users" };
        private UsersViewModel users = new UsersViewModel();
        #endregion
        #region Change Menu
        public string[] MainMenu { get { return mainMenu; } }

        private string[] currentMenu;
        public string[] CurrentMenu
        {
            get { return currentMenu; }
            set { SetProperty(ref currentMenu, value); }
        }

        private ICommand switchMenuCommand;
        public ICommand SwitchMenuCommand
        {
            get
            {
                if (switchMenuCommand == null) switchMenuCommand = new RelayCommand(new Action<object>(SwitchMenu));
                return switchMenuCommand;
            }
            set { SetProperty(ref switchMenuCommand, value); }
        }

        private void SwitchMenu(object parameter)
        {
            string destination = (string)parameter;
            //NotificationProvider.Info("Switch menu", destination);
            switch (destination)
            {
                case "Tables":
                    CurrentMenu = tablesMenu;
                    break;
                case "Transactions":
                    CurrentMenu = transactionsMenu;
                    break;
                case "Lists":
                    CurrentMenu = listsMenu;
                    break;
                case "Settings":
                    CurrentMenu = settingsMenu;
                    break;
                default:
                    CurrentMenu = null;
                    break;
            }
            CurrentViewModel = null;
        }
        #endregion
        #region Change View
        private BindableBase currentViewModel;
        public BindableBase CurrentViewModel
        {
            get
            {
                //if (currentViewModel == null) currentViewModel = new TablesMenuViewModel();
                return currentViewModel;
            }
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
            //NotificationProvider.Info("Navigate", destination);
            switch (destination)
            {
                case "Products":
                    CurrentViewModel = products;
                    break;
                case "Product categories":
                    CurrentViewModel = productCategories;
                    break;
                case "Partners":
                    CurrentViewModel = partners;
                    break;
                case "Users":
                    CurrentViewModel = users;
                    break;
                case "Incoming":
                    CurrentViewModel = incomingTransactions;
                    break;
                case "Outgoing":
                    CurrentViewModel = outgoingTransactions;
                    break;
                case "Inventory":
                    CurrentViewModel = inventory;
                    break;
                case "Partner transactions":
                    CurrentViewModel = partnerTransactions;
                    break;
                default:
                    CurrentViewModel = null;
                    break;
            }
        }
        #endregion
        #region Close Main window
        private Window mainWindow;
        public Window MainWindow
        {
            get { return mainWindow; }
            set { SetProperty(ref mainWindow, value); }
        }
        private ICommand closeMainWindowCommand;
        public ICommand CloseMainWindowCommand
        {
            get
            {
                if (closeMainWindowCommand == null) closeMainWindowCommand = new RelayCommand(CloseWindow);
                return closeMainWindowCommand;
            }
            set { SetProperty(ref closeMainWindowCommand, value); }
        }
        private void CloseWindow(object parameter = null)
        {
            MainWindow?.Close();
        }
        #endregion
    }
}
