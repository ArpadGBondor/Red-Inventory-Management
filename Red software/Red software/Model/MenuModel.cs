using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Red_software.Model
{
    public abstract class MenuModel : BindableBase
    {

        private List<String> buttons;
        public List<String> Buttons
        {
            get
            {
                if (buttons == null) buttons = new List<string>();
                return buttons;
            }
            set { SetProperty(ref buttons, value); }
        }

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

        protected abstract void Navigate(object parameter);
    }
}
