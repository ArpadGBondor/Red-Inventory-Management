using System.Windows;
using BusinessLayer;
using Red_software.Notifications;

namespace Red_software.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            // Set database connection
            if (!DatabaseConnection.TestConnection())
            {
                SetupConnectionWindow SCW = new SetupConnectionWindow();
                SCW.ShowDialog();
                if (!DatabaseConnection.TestConnection())
                    this.Close();
            }

            // New user
            if (UserLogin.IsEmptyUserDatabase())
            {
                NotificationProvider.Info("Welcome First User!", "Please, set a username and a password.");
                NewUserWindow NUW = new NewUserWindow();
                NUW.ShowDialog();
                if (UserLogin.IsEmptyUserDatabase())
                    this.Close();
            }
            
            // Login
            LoginWindow LW = new LoginWindow();
            LW.ShowDialog();
            if (UserLogin.LoginedUser == "") // Not logged in
                this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NotificationProvider.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NotificationProvider.Error("Error!", "Error text");
            NotificationProvider.Alert("Alert!", "Alert text");
            NotificationProvider.Info("Info!", "Info text");
        }
    }
}
