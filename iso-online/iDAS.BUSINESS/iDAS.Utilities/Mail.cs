using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Utilities
{
    public class MailSettingItem
    {
        public bool EnableSSL { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool IsBodyHtml { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
    public class Mail : MailSettingItem
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] EmailToAddress { get; set; }
        public string[] EmailToAddressCC { get; set; }
    }
    public class MailSendInfo
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] EmailToAddress { get; set; }
        public string[] EmailToAddressCC { get; set; }
    }
    public class Mails : MailSettingItem
    {
       public List<MailSendInfo> SendInfo { get; set; }
    }
}
