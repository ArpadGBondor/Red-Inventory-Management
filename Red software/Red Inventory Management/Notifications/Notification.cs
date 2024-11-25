using Red_Inventory_Management.Model;
using System.Collections.ObjectModel;

namespace Red_Inventory_Management.Notifications
{
    public class Notification : BindableBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { SetProperty(ref _imageUrl, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _bgcolor;
        public string BGColor
        {
            get
            {
                if (_bgcolor == null) _bgcolor = "#2a3345";
                return _bgcolor;
            }
            set { SetProperty(ref _bgcolor, value); }
        }

        private string _textcolor;
        public string TextColor
        {
            get
            {
                if (_textcolor == null) _textcolor = "White";
                return _textcolor;
            }
            set { SetProperty(ref _textcolor, value); }
        }
    }

    public class NotificationCollection : ObservableCollection<Notification> { }

}
