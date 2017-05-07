using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Red_Inventory_Management.Model
{
    public abstract class EditItemModel<Entity> : BindableBase
    {
        private EditItemModel() { }
        protected EditItemModel(Entity item, bool newRecord, string itemName) { Item = item; NewRecord = newRecord; ItemName = itemName; }

        private string _itemName;
        public string ItemName
        {
            get { return _itemName; }
            set { SetProperty(ref _itemName, value); }
        }

        public string TitleText { get { return (NewRecord ? "New " : "Edit ") + ItemName; } }
        public string OkButtonText { get { return (NewRecord ? "Add " : "Save ") + ItemName; } }

        private Entity _item;
        public Entity Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }
        private bool _newRecord;
        public bool NewRecord
        {
            get { return _newRecord; }
            set { SetProperty(ref _newRecord, value); }
        }

        private bool _saveEdit = false;
        public bool SaveEdit
        {
            get { return _saveEdit; }
            set { SetProperty(ref _saveEdit, value); }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null) _saveCommand = new RelayCommand(new Action<object>(Validate), new Predicate<object>(SaveCanExecute));
                return _saveCommand;
            }
            set { SetProperty(ref _saveCommand, value); }
        }
        private void Validate(object parameter)
        {
            if (Save(parameter))
            {
                SaveEdit = true;
                Close(parameter);

            }
        }
        protected virtual bool SaveCanExecute(object parameter) { return true; }

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null) _closeCommand = new RelayCommand(new Action<object>(Close), new Predicate<object>(CloseCanExecute));
                return _closeCommand;
            }
            set { SetProperty(ref _closeCommand, value); }
        }
        private void Close(object parameter)
        {
            ((Window)parameter).Close();
        }
        protected virtual bool CloseCanExecute(object parameter) { return true; }

        protected abstract bool Save(object parameter);

    }

}
