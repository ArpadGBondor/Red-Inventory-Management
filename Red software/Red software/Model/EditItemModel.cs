using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Red_software.Model
{
    public class EditItemModel<Entity> : BindableBase
    {

        private EditItemModel() { }
        protected EditItemModel(Entity _Item, bool _NewRecord) { Item = _Item; NewRecord = _NewRecord; }

        public string TitleText { get { return (NewRecord ? "New record" : "Edit record"); } }
        public string OkButtonText { get { return (NewRecord ? "Add record" : "Save record"); } }

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
                if (saveCommand == null) saveCommand = new RelayCommand(new Action<object>(Save), new Predicate<object>(SaveCanExecute));
                return saveCommand;
            }
            set { SetProperty(ref saveCommand, value); }
        }
        protected virtual void Save(object parameter) { SaveEdit = true; Close(parameter); }
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
        protected virtual void Close(object parameter) { ((Window)parameter).Close(); }
        protected virtual bool CloseCanExecute(object parameter) { return true; }

    }

}
