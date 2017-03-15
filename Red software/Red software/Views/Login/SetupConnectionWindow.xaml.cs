using System;
using System.Windows;
using Microsoft.Win32;
using BusinessLayer;
using Red_software.ViewModel;

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
            SetupConnectionViewModel context = new SetupConnectionViewModel();
            this.DataContext = context;
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
