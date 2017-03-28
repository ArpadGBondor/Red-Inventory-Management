using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Red_software.Model
{
    public abstract class EditItemModel<Entity> : BindableBase
    {

        private EditItemModel() { }
        protected EditItemModel(Entity _Item, bool _NewRecord, string _ItemName/* = "record"*/) { Item = _Item; NewRecord = _NewRecord; ItemName = _ItemName; }

        private string itemName;
        public string ItemName
        {
            get { return itemName; }
            set { SetProperty(ref itemName, value); }
        }

        public string TitleText { get { return (NewRecord ? "New " : "Edit ") + ItemName; } }
        public string OkButtonText { get { return (NewRecord ? "Add " : "Save ") + ItemName; } }

        private Entity item;
        public Entity Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }
        private bool newRecord;
        public bool NewRecord
        {
            get { return newRecord; }
            set { SetProperty(ref newRecord, value); }
        }

        private bool saveEdit = false;
        public bool SaveEdit
        {
            get { return saveEdit; }
            set { SetProperty(ref saveEdit, value); }
        }

        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null) saveCommand = new RelayCommand(new Action<object>(Validate), new Predicate<object>(SaveCanExecute));
                return saveCommand;
            }
            set { SetProperty(ref saveCommand, value); }
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

        private ICommand closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (closeCommand == null) closeCommand = new RelayCommand(new Action<object>(Close), new Predicate<object>(CloseCanExecute));
                return closeCommand;
            }
            set { SetProperty(ref closeCommand, value); }
        }
        private void Close(object parameter) { ((Window)parameter).Close(); }
        protected virtual bool CloseCanExecute(object parameter) { return true; }

        protected abstract bool Save(object parameter);

    }

}
