using System;
using System.Windows.Input;

namespace Red_Inventory_Management.Model
{
    public abstract class TableModel<Entity> : ListModel<Entity>
    {
        private ICommand _editItemCommand;
        public ICommand EditItemCommand
        {
            get
            {
                if (_editItemCommand == null) _editItemCommand = new RelayCommand(new Action<object>(EditItem), new Predicate<object>(EditItemCanExecute));
                return _editItemCommand;
            }
            set { SetProperty(ref _editItemCommand, value); }
        }

        private ICommand _newItemCommand;
        public ICommand NewItemCommand
        {
            get
            {
                if (_newItemCommand == null) _newItemCommand = new RelayCommand(new Action<object>(NewItem), new Predicate<object>(NewItemCanExecute));
                return _newItemCommand;
            }
            set { SetProperty(ref _newItemCommand, value); }
        }

        private ICommand _deleteItemCommand;
        public ICommand DeleteItemCommand
        {
            get
            {
                if (_deleteItemCommand == null) _deleteItemCommand = new RelayCommand(new Action<object>(DeleteItem), new Predicate<object>(DeleteItemCanExecute));
                return _deleteItemCommand;
            }
            set { SetProperty(ref _deleteItemCommand, value); }
        }

        protected abstract void NewItem(object parameter);
        protected abstract void DeleteItem(object parameter);
        protected abstract void EditItem(object parameter);

        protected virtual bool NewItemCanExecute(object parameter)
        {
            return true;
        }
        protected virtual bool EditItemCanExecute(object parameter)
        {
            return ItemSelected(parameter);
        }
        protected virtual bool DeleteItemCanExecute(object parameter)
        {
            return ItemSelected(parameter);
        }

        public string newButtonContent { get { return string.Format("New {0}", ItemName); } }
        public string editButtonContent { get { return string.Format("Edit {0}", ItemName); } }
        public string deleteButtonContent { get { return string.Format("Delete {0}", ItemName); } }

    }
}