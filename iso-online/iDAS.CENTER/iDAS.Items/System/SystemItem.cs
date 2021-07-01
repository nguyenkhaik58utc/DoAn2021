using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class SystemItem
    {
        private string _DBSource = string.Empty;
        private string _DBUserID = string.Empty;
        private string _DBPassword = string.Empty;
        private string _MailHost = string.Empty;
        private string _MailUserName = string.Empty;
        private string _MailUserPassword = string.Empty;
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string DocumentFolder { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string DBSource
        {
            get
            {
                return Encryptor.Decode(_DBSource);
            }
            set
            {
                _DBSource = value;
            }
        }
        public string DBUserID
        {
            get
            {
                return Encryptor.Decode(_DBUserID);
            }
            set
            {
                _DBUserID = value;
            }
        }
        public string DBPassword
        {
            get
            {
                return Encryptor.Decode(_DBPassword);
            }
            set
            {
                _DBPassword = value;
            }
        }
        public string MailHost
        {
            get
            {
                return string.IsNullOrEmpty(_MailHost) ? null : Encryptor.Decode(_MailHost);
            }
            set
            {
                _MailHost = value;
            }
        }
        public Nullable<int> MailPort { get; set; }
        public Nullable<bool> MailEnableSsl { get; set; }
        public string MailUserName
        {
            get
            {
                return string.IsNullOrEmpty(_MailUserName) ? null : Encryptor.Decode(_MailUserName);
            }
            set
            {
                _MailUserName = value;
            }
        }
        public string MailUserPassword
        {
            get
            {
                return string.IsNullOrEmpty(_MailUserPassword) ? null : Encryptor.Decode(_MailUserPassword);
            }
            set
            {
                _MailUserPassword = value;
            }
        }
        public byte[] ImageUser { get; set; }
        public string ImageUserUrl
        {
            get
            {
                var url = "data:image;base64,{0}";
                if (ImageUser == null) return "/Content/images/underfind.jpg";
                var data = Convert.ToBase64String(ImageUser);
                url = string.Format(url, data);
                return url;
            }
        }
        public byte[] ImageLogo { get; set; }
        public string ImageLogoUrl
        {
            get
            {
                var url = "data:image;base64,{0}";
                if (ImageLogo == null) return "../../Content/images/iDAS-logo.png";
                var data = Convert.ToBase64String(ImageLogo);
                url = string.Format(url, data);
                return url;
            }
        }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
