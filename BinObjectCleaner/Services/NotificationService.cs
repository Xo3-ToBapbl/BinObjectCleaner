using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using BinObjectCleaner.Utilities;

namespace BinObjectCleaner.Services
{
    public class NotificationService
    {
        public void SendToast(string message)
        {
            ToastVisual visual = new ToastVisual
            {
                BindingGeneric = new ToastBindingGeneric
                {
                    Children =
                    {
                        new AdaptiveText { Text=Constants.AppName },
                        new AdaptiveText { Text=message },
                    }
                },
            };
            ToastContent content = new ToastContent
            {
                Visual = visual,
            };
            ToastNotification notification = new ToastNotification(content.GetXml())
            {
                ExpirationTime = DateTime.Now.AddMinutes(1),
            };

            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }
    }
}
