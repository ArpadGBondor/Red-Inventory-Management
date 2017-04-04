using System.Collections.ObjectModel;
using System.ComponentModel;
using Red_Inventory_Management;
using Red_Inventory_Management.Model;

namespace Red_Inventory_Management.Notifications
{
    public class Notification : BindableBase
    {
        private string message;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        private int id;
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get { return imageUrl; }
            set { SetProperty(ref imageUrl, value); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private string bgcolor;
        public string BGColor
        {
            get
            {
                if (bgcolor == null) bgcolor = "#2a3345";
                return bgcolor;
            }
            set { SetProperty(ref bgcolor, value); }
        }

        private string textcolor;
        public string TextColor
        {
            get
            {
                if (textcolor == null) textcolor = "White";
                return textcolor;
            }
            set { SetProperty(ref textcolor, value); }
        }
    }

    public class NotificationCollection : ObservableCollection<Notification> { }

}
