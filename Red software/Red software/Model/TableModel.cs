using Red_software.Model;
using Red_software.Notifications;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Red_software.Model
{
    // Needed a Parent class, without template parameter to execute the refresh command from the View
    public abstract class TableModelParent : BindableBase
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

        protected abstract bool ItemSelected(object parameter);

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

        public void ResizeGridViewColumn(GridViewColumn column)
        {
            if (double.IsNaN(column.Width))
            {
                column.Width = column.ActualWidth;
            }

            column.Width = double.NaN;
        }
    }

    public abstract class TableModel<Entity> : TableModelParent
    {
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
        protected override bool ItemSelected(object parameter)
        {
            return (SelectedItem != null);
        }
    }
}