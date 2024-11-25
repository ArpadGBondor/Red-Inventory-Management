﻿using Red_Inventory_Management.ViewModel;
using System.Windows;

namespace Red_Inventory_Management.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            LoginViewModel context = new LoginViewModel();
            context.LoginWindow = this;
            this.DataContext = context;
        }
    }
}
