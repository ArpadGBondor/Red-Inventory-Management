﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Red_Inventory_Management.Notifications
{
    public partial class GrowlNotifiactions
    {
        private const byte _MAX_NOTIFICATIONS = 6;
        private int _count;
        public NotificationCollection Notifications = new NotificationCollection();
        private readonly NotificationCollection _buffer = new NotificationCollection();

        public GrowlNotifiactions()
        {
            InitializeComponent();
            NotificationsControl.DataContext = Notifications;
        }

        public void AddNotification(Notification notification)
        {
            notification.Id = _count++;
            if (Notifications.Count + 1 > _MAX_NOTIFICATIONS)
                _buffer.Add(notification);
            else
                Notifications.Add(notification);

            //Show window if there're notifications
            if (Notifications.Count > 0 && !IsActive)
                Show();
        }

        public void RemoveNotification(Notification notification)
        {
            if (Notifications.Contains(notification))
                Notifications.Remove(notification);

            if (_buffer.Count > 0)
            {
                Notifications.Add(_buffer[0]);
                _buffer.RemoveAt(0);
            }

            //Close window if there's nothing to show
            if (Notifications.Count < 1)
                Hide();
        }

        private void NotificationWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height != 0.0)
                return;
            var element = sender as Grid;
            RemoveNotification(Notifications.First(n => n.Id == Int32.Parse(element.Tag.ToString())));
        }
    }
}
