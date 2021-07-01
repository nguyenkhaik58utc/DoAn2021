using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Ext.Net;

namespace iDAS.Utilities
{
    public class Ultility
    {
        // HungNM. Add errorCode for logging in.
        public static string strerrorCode;

        // End.

        public static string GetIconFile(string fileType)
        {
            var iconName = "";
            switch (fileType)
            {
                case "doc":
                    iconName = "PageWord"; break;
                case "docx":
                    iconName = "PageWord"; break;
                case "xls":
                    iconName = "PageExcel"; break;
                case "xlsx":
                    iconName = "PageExcel"; break;
                case "pdf":
                    iconName = "PageWhiteAcrobat"; break;
                case "jpg":
                    iconName = "Image"; break;
                case "jpeg":
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

        public static void ShowNotification(string title = RequestMessage.Notify, string message = "", Icon icon = Icon.Information, bool error = false, int width = 270, int height = 80, int hideDelay = 3000)
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
            });
            ntf.Show();
        }

        public static void ShowMessageBox(string title = RequestMessage.Notify, string message = "", MessageBox.Icon icon = MessageBox.Icon.NONE, MessageBox.Button button = MessageBox.Button.OK)
        {
            MessageBox msg = new MessageBox();
            msg.Configure(new MessageBoxConfig()
            {
                Title = title,
                Message = message,
                Icon = icon,
                Buttons = button,
            });
            msg.Show();
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

        public static Icon ConvertToIcon(string iconName)
        {
            if (string.IsNullOrEmpty(iconName)) iconName = Icon.None.ToString();
            return (Icon)Enum.Parse(typeof(Icon), iconName);
        }

        public static bool CheckFileImage(string file)
        {
            string type = file.Split('.').Last().ToLower();

            switch (type)
            {
                case "jpg": return true;
                case "jpeg": return true;
                case "jpe": return true;
                case "jfif": return true;
                case "bmp": return true;
                case "dib": return true;
                case "gif": return true;
                case "png": return true;
                case "ico": return true;
                case "tif": return true;
                case "tiff": return true;
                default: return false;
            }
        }

        public static bool CheckFormatFile(string file, FormatFile format)
        {
            switch (format)
            {
                case FormatFile.Image: return CheckFileImage(file);
            }
            return false;
        }

        public static decimal? ConvertToDecimal(string value)
        {
            value = value.Replace(",", "");
            value = value.Replace(".", ",");
            if (Regex.IsMatch(value, @"^\d\,$"))
                return Decimal.Parse(value);
            else
                return null;
        }

        public static List<object> ConvertToListInt(string value)
        {
            var source = value.Split(',');
            List<object> result = new List<object>();
            foreach (var item in source)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var id = System.Convert.ToInt32(item);
                    result.Add(id);
                }
            }
            return result;
        }

        // HungNM. Add errorCode for logging in.
        public static void SetErrorCodeLogin(string value)
        {
            strerrorCode = value;
        }

        public static string GetErrorCodeLogin()
        {
            return strerrorCode;
        }

        // End.

        public static string[] Name = new[] { "Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy" };

        public static string GetNameDayOfWeek(DateTime date)
        {
            int day = (int)date.DayOfWeek;
            return Name[day];
        }
    }
}