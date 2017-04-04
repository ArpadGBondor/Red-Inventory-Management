using System;
using System.Windows;
using Microsoft.Win32;
using BusinessLayer;
using Red_Inventory_Management.ViewModel;

namespace Red_Inventory_Management.Views
{
    /// <summary>
    /// Interaction logic for SetupConnectionWindow.xaml
    /// </summary>
    public partial class SetupConnectionWindow : Window
    {
        public SetupConnectionWindow()
        {
            InitializeComponent();
            SetupConnectionViewModel context = new SetupConnectionViewModel() { SetupWindow = this };
            this.DataContext = context;
        }
    }
}
