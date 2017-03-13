using System.Windows;
using BusinessLayer;

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
                NewUserWindow NUW = new NewUserWindow();
                NUW.ShowDialog();
                if (UserLogin.IsEmptyUserDatabase())
                    this.Close();
            }
            
            // Login
            LoginWindow LW = new LoginWindow();
            LW.ShowDialog();
            if (UserLogin.UserID == "") // Not logged in
                this.Close();
        }
    }
}
