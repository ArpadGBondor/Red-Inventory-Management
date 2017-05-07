using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Red_Inventory_Management.Notifications
{
    class NotificationProvider
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const double _topOffset = 20;
        private const double _leftOffset = 480;
        public static GrowlNotifiactions growlNotifications { get; set; }

        static NotificationProvider()
        {
            growlNotifications = new GrowlNotifiactions();
            growlNotifications.Top = SystemParameters.WorkArea.Top + _topOffset;
            growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - _leftOffset;
        }

        public static void Error(string msgTitle, string msgMessage)
        {
            log.Info(string.Format("Error notification\nTitle: {0}\nMessage: {1}",msgTitle,msgMessage));
            growlNotifications.AddNotification(
                new Notification {
                    TextColor = "Red",
                    BGColor = "#FFFFB0",
                    Title = msgTitle,
                    ImageUrl = "pack://application:,,,/Notifications/Resources/error.png",
                    Message = msgMessage
                });
        }

        public static void Alert(string msgTitle, string msgMessage)
        {
            log.Info(string.Format("Alert notification\nTitle: {0}\nMessage: {1}", msgTitle, msgMessage));
            growlNotifications.AddNotification(
                new Notification
                {
                    TextColor = "Orange",
                    BGColor = "Black",
                    Title = msgTitle,
                    ImageUrl = "pack://application:,,,/Notifications/Resources/alert.png",
                    Message = msgMessage
                });
        }

        public static void Info(string msgTitle, string msgMessage)
        {
            log.Info(string.Format("Info notification\nTitle: {0}\nMessage: {1}", msgTitle, msgMessage));
            growlNotifications.AddNotification(
                new Notification
                {
                    TextColor = "Blue",
                    BGColor = "Silver",
                    Title = msgTitle,
                    ImageUrl = "pack://application:,,,/Notifications/Resources/info.png",
                    Message = msgMessage
                });
        }

        public static void Close()
        {
            growlNotifications.Close();
        }

    }
}
