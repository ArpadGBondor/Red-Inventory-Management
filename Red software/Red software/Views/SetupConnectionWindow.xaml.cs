using System;
using System.Windows;
using Microsoft.Win32;
using BusinessLayer;

namespace Red_software.Views
{
    /// <summary>
    /// Interaction logic for SetupConnectionWindow.xaml
    /// </summary>
    public partial class SetupConnectionWindow : Window
    {
        public SetupConnectionWindow()
        {
            InitializeComponent();
            FileNameTextBox.Text = AppDomain.CurrentDomain.BaseDirectory + "Database.mdf";
            TestConnection();
            
        }

        public bool TestConnection()
        {
            bool connected = DatabaseConnection.TestConnection();
            ConnectionStateLabel.Content = connected ? "Connected" : "Not connected";
            return connected;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            FileNameTextBox.Text = openFileDialog.FileName;
            if (openFileDialog.ShowDialog() == true)
            {
                FileNameTextBox.Text = openFileDialog.FileName;
                DatabaseConnection.ChangeDatabaseFile(FileNameTextBox.Text);
                TestConnection();
            }
        }

    }
}
