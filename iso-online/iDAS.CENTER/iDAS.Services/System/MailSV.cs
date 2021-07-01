using iDAS.Core;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class MailSV
    {
        private static SmtpClient smtpClient;
        private static MailAddress from;
        private SmtpClient getSmtpClient()
        {
            if (smtpClient == null)
            {
                SystemSV systemSV = new SystemSV();
                var mail = systemSV.GetMail(BaseSystem.SystemCode);
                smtpClient = new SmtpClient(mail.Host, mail.Port.Value);
                smtpClient.EnableSsl = mail.EnableSsl.Value;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(mail.UserName, mail.UserPassword);
                from = new MailAddress(mail.UserName);
            }
            return smtpClient;
        }
        public bool SendMail(MailMessage mail)
        {
            try
            {
                SmtpClient client = getSmtpClient();
                mail.From = from;
                client.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
