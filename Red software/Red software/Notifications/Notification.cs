using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Red_software.Notifications
{
    public class Notification : INotifyPropertyChanged
    {
        public Notification()
        {
            BGColor = "#2a3345";
            TextColor = "White";
        }
        private string message;
        public string Message
        {
            get { return message; }

            set
            {
                if (message == value) return;
                message = value;
                OnPropertyChanged("Message");
            }
        }

        private int id;
        public int Id
        {
            get { return id; }

            set
            {
                if (id == value) return;
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get { return imageUrl; }

            set
            {
                if (imageUrl == value) return;
                imageUrl = value;
                OnPropertyChanged("ImageUrl");
            }
        }

        private string title;
        public string Title
        {
            get { return title; }

            set
            {
                if (title == value) return;
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private string bgcolor;
        public string BGColor
        {
            get { return bgcolor; }
            set
            {
                if (bgcolor == value) return;
                bgcolor = value;
                OnPropertyChanged("BGColor");
            }
        }

        private string textcolor;
        public string TextColor
        {
            get { return textcolor; }
            set
            {
                if (textcolor == value) return;
                textcolor = value;
                OnPropertyChanged("TextColor");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class NotificationCollection : ObservableCollection<Notification> { }

}
