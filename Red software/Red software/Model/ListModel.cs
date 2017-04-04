using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Red_Inventory_Management.Model
{
    // Needed a base class without template parameter to call the RefreshListCommand from the view
    public abstract class ListModel<Entity> : BindableBase
    {
        private ICommand refreshListCommand;
        public ICommand RefreshListCommand
        {
            get
            {
                if (refreshListCommand == null) refreshListCommand = new RelayCommand(new Action<object>(RefreshList));
                return refreshListCommand;
            }
            set { SetProperty(ref refreshListCommand, value); }
        }
        protected abstract void RefreshList(object parameter);

        private string tableName;
        public string TableName
        {
            get
            {
                if (tableName == null) tableName = "Table";
                return tableName;
            }
            set { SetProperty(ref tableName, value); }
        }

        private List<Entity> list;
        public List<Entity> List
        {
            get { return list; }
            set { SetProperty(ref list, value); }
        }

        private Entity selectedItem;
        public Entity SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); }
        }
        protected virtual bool ItemSelected(object parameter)
        {
            return (SelectedItem != null);
        }

        private string itemName;
        public string ItemName
        {
            get
            {
                if (itemName == null) itemName = "record";
                return itemName;
            }
            set { SetProperty(ref itemName, value); }
        }
    }
}
