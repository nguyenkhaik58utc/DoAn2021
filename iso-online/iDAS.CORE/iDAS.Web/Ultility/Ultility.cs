using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Ext.Net;
using iDAS.Core;
using iDAS.Items;
namespace iDAS.Web
{
    public class Ultility 
    {
        public static void SetStatus<T>(List<T> source) where T : BaseSystemItem
        {
            foreach (var item in source)
            {
                switch (item.Status) 
                {
                    //case (byte)SystemCommon.Status.Obsolete: item.StatusView = Status.Obsolete; break;
                    //case (byte)SystemCommon.Status.Using: item.StatusView = Status.Using; break;
                    //case (byte)SystemCommon.Status.New: item.StatusView = Status.New; break;
                    default: item.StatusView = string.Empty; break;
                }
            }
        }

        public static string GetIconUrl(string iconName)
        {
            iconName = string.IsNullOrEmpty(iconName) ? Icon.None.ToString() : iconName;
            var resource = new ResourceManager();
            var iconUrl = resource.GetIconUrl((Icon)Enum.Parse(typeof(Icon), iconName));
            return iconUrl;
        }

        public static string GetIconName(string iconUrl)
        {
            var icons = GetIcons();
            var iconName = (from icon in icons
                            where icon.IconUrl == iconUrl
                            select icon.IconName).FirstOrDefault();
            iconName = string.IsNullOrEmpty(iconName) ? Icon.None.ToString() : iconName;
            return iconName;
        }

        public static void SetIconUrl<T>(List<T> source) where T : SystemIconItem
        {
            foreach (var item in source) {
                item.IconUrl = GetIconUrl(item.IconName);
            }
        }

        public static void SetIconName<T>(T item) where T : SystemIconItem 
        {
            item.IconName = GetIconName(item.IconUrl);
        }

        public static List<SystemIconItem> GetIcons()
        {
            var source = (from i in Enum.GetNames(typeof(Icon)) select i).ToList();
            var icons = new List<SystemIconItem>();
            foreach (var item in source)
            {
                icons.Add(new SystemIconItem()
                {
                    IconName = item,
                    IconUrl = GetIconUrl(item),
                });
            }
            return icons;
        }

        public static IEnumerable GetIcons(int start, int limit, out int total)
        {
            var data = Ultility.GetIcons();
            var icons = new List<SystemIconItem>();
            total = data.Count;
            for (int i = start + 1; i <= start + limit; i++)
            {
                if (i - 1 >= data.Count) break;
                var icon = new SystemIconItem()
                {
                    IconName = data[i - 1].IconName,
                    IconUrl = data[i - 1].IconUrl,
                };

                icons.Add(icon);
            }

            return icons;
        }

        public static Notification CreateNotification(string title = Message.Notify, string message="", Icon icon = Icon.Information, bool error = false, int width = 270, int height = 80, int hideDelay = 3000)
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

        public static MessageBox CreateMessageBox(string title,string message,MessageBox.Icon icon=MessageBox.Icon.NONE, MessageBox.Button button=MessageBox.Button.OK) {
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

        public static void ShowNotification(string title = Message.Notify, string message = "", Icon icon = Icon.Information, bool error = false, int width = 270, int height = 80, int hideDelay = 3000)
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

        public static void ShowMessageBox(string title = Message.Notify, string message ="", MessageBox.Icon icon = MessageBox.Icon.NONE, MessageBox.Button button = MessageBox.Button.OK)
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

        public static Icon ConvertToIcon(string iconName) {
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

        public static bool CheckFormatFile(string file, FormatFile format) {
            switch (format)
            {
                case FormatFile.Image: return CheckFileImage(file);
            }
            return false;
        }

        public static decimal? ConvertToDecimal(string value){
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
                    var id = Convert.ToInt32(item);
                    result.Add(id);
                }
            }
            return result;
        }
    }
}