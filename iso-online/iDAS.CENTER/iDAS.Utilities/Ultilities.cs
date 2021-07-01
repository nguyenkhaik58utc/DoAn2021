using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Utilities
{
    public class Ultilities
    {
        public static string GetIconFile(string fileType)
        {
            var iconName = "";
            switch (fileType)
            {
                case "doc":
                    iconName = "PageWord"; break;
                case "docx":
                    iconName = "PageWord"; break;
                case "jpg":
                    iconName = "Image"; break;
                case "png":
                    iconName = "Image"; break;
                default: iconName = "Attach"; break;
            }
            return GetIconUrl(iconName);
        }
        public static string GetIconUrl(string iconName)
        {
            iconName = string.IsNullOrEmpty(iconName) ? Icon.None.ToString() : iconName;
            var resource = new ResourceManager();
            var iconUrl = resource.GetIconUrl((Icon)Enum.Parse(typeof(Icon), iconName));
            return iconUrl;
        }

        public static Notification CreateNotification(string title = RequestMessage.Notify, string message = "", Icon icon = Icon.Information, bool error = false, int width = 270, int height = 80, int hideDelay = 3000)
        {
            Notification ntf = new Notification();
            ntf.Configure(new NotificationConfig()
            {
                Title = title,
                Html = message,
                Width = width,
                Height = height,
                HideDelay = hideDelay,
                Icon = error ? Icon.Exclamation : icon,
            }).Show();
            return ntf;
        }

        public static MessageBox CreateMessageBox(string title, string message, MessageBox.Icon icon = MessageBox.Icon.NONE, MessageBox.Button button = MessageBox.Button.OK)
        {
            MessageBox msg = new MessageBox();
            msg.Configure(new MessageBoxConfig()
            {
                Title = title,
                Message = message,
                Icon = icon,
                Buttons = button,
            }).Show();
            return msg;
        }

        public static void ShowMessageRequest(RequestStatus status)
        {
            var field = status.ToString();
            var message = typeof(RequestMessage).GetField(field).GetRawConstantValue().ToString();
            var icon = field.Contains("Error") ? MessageBox.Icon.ERROR : MessageBox.Icon.INFO;
            MessageBox msg = new MessageBox();
            msg.Configure(new MessageBoxConfig()
            {
                Title = RequestMessage.Notify,
                Message = message,
                Icon = icon,
                Buttons = MessageBox.Button.OK,
            });
            msg.Show();
        }
    }
}
