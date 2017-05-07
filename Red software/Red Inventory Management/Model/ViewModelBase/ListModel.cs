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
        private ICommand _refreshListCommand;
        public ICommand RefreshListCommand
        {
            get
            {
                if (_refreshListCommand == null) _refreshListCommand = new RelayCommand(new Action<object>(RefreshList));
                return _refreshListCommand;
            }
            set { SetProperty(ref _refreshListCommand, value); }
        }
        protected abstract void RefreshList(object parameter);

        private string _tableName;
        public string TableName
        {
            get
            {
                if (_tableName == null) _tableName = "Table";
                return _tableName;
            }
            set { SetProperty(ref _tableName, value); }
        }

        private List<Entity> _list;
        public List<Entity> List
        {
            get { return _list; }
            set { SetProperty(ref _list, value); }
        }

        private Entity _selectedItem;
        public Entity SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        protected virtual bool ItemSelected(object parameter)
        {
            return (SelectedItem != null);
        }

        private string _itemName;
        public string ItemName
        {
            get
            {
                if (_itemName == null) _itemName = "record";
                return _itemName;
            }
            set { SetProperty(ref _itemName, value); }
        }
    }
}
