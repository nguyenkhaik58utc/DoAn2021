using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class MailItem
    {
        private bool _EnableSsl = false;
        private string _Host = string.Empty;
        private string _UserName = string.Empty;
        private string _UserPassword = string.Empty;
        public int? Port { get; set; }
        public bool? EnableSsl
        {
            get
            {
                return _EnableSsl;

            }
            set
            {
                _EnableSsl = value.HasValue ? (bool) value : false;
            }
        }
        public string Host
        {
            get
            {
                return string.IsNullOrEmpty(_Host) ? null : Encryptor.Decode(_Host);
            }
            set
            {
                _Host = value;
            }
        }
        public string UserName
        {
            get
            {
                return string.IsNullOrEmpty(_UserName) ? null : Encryptor.Decode(_UserName);
            }
            set
            {
                _UserName = value;
            }
        }
        public string UserPassword
        {
            get
            {
                return string.IsNullOrEmpty(_UserPassword) ? null : Encryptor.Decode(_UserPassword);
            }
            set
            {
                _UserPassword = value;
            }
        }
    }
}
