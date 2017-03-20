using Red_software.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Red_software.ViewModel
{
    public class TablesMenuViewModel : BindableBase
    {

        #region Views
        ProductsViewModel products = new ProductsViewModel();
        ProductCategoriesViewModel productCategories = new ProductCategoriesViewModel();
        #endregion
        #region Change Views
        private BindableBase currentViewModel;
        public BindableBase CurrentViewModel
        {
            get
            {
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
            switch (destination)
            {
                case "Products":
                    CurrentViewModel = products;
                    break;
                case "Product categories":
                    CurrentViewModel = productCategories;
                    break;
                default:
                    break;
            }
        }
        #endregion


    }
}
