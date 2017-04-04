using Red_Inventory_Management.Model;
using Red_Inventory_Management.Notifications;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Red_Inventory_Management.Model
{
    public abstract class TableModel<Entity> : ListModel<Entity>
    {
        private ICommand editItemCommand;
        public ICommand EditItemCommand
        {
            get
            {
                if (editItemCommand == null) editItemCommand = new RelayCommand(new Action<object>(EditItem), new Predicate<object>(EditItemCanExecute));
                return editItemCommand;
            }
            set { SetProperty(ref editItemCommand, value); }
        }

        private ICommand newItemCommand;
        public ICommand NewItemCommand
        {
            get
            {
                if (newItemCommand == null) newItemCommand = new RelayCommand(new Action<object>(NewItem), new Predicate<object>(NewItemCanExecute));
                return newItemCommand;
            }
            set { SetProperty(ref newItemCommand, value); }
        }

        private ICommand deleteItemCommand;
        public ICommand DeleteItemCommand
        {
            get
            {
                if (deleteItemCommand == null) deleteItemCommand = new RelayCommand(new Action<object>(DeleteItem), new Predicate<object>(DeleteItemCanExecute));
                return deleteItemCommand;
            }
            set { SetProperty(ref deleteItemCommand, value); }
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