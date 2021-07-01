using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Utilities
{
    public static class MailService
    {
        public static string SendEmail(this Mail mail)
        {
            if (mail.UserName == null || mail.UserName.Trim().Length == 0)
            {
                return "User Name Empty!";
            }
            if (mail.UserPassword == null || mail.UserPassword.Trim().Length == 0)
            {
                return "Email Password Empty!";
            }
            if (mail.EmailToAddress == null || mail.EmailToAddress.Length == 0)
            {
                return "Email To Address Empty!";
            }

            List<string> tempFiles = new List<string>();

            SmtpClient smtpClient = new SmtpClient(mail.Host, mail.Port);
            smtpClient.EnableSsl = mail.EnableSSL;

            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail.UserName, mail.UserPassword);
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(mail.UserName);
                message.Subject = mail.Subject == null ? "" : mail.Subject;
                message.Body = mail.Body == null ? "" : mail.Body;
                message.IsBodyHtml = mail.IsBodyHtml;

                foreach (string email in mail.EmailToAddress)
                {
                    message.To.Add(email);
                }
                if (mail.EmailToAddressCC != null && mail.EmailToAddressCC.Length > 0)
                {
                    foreach (string emailCc in mail.EmailToAddressCC)
                    {
                        if (!string.IsNullOrEmpty(emailCc))
                            message.CC.Add(emailCc);
                    }
                }

                try
                {
                    smtpClient.Send(message);
                    return "Email Send SuccessFully";
                }
                catch (Exception ex)
                {
                    return "Email Send failed";
                }
            }
        }
        public static void SendMail(this Mails mails)
        {
            if (mails.UserName == null || mails.UserName.Trim().Length == 0)
            {
                return;
            }
            if (mails.UserPassword == null || mails.UserPassword.Trim().Length == 0)
            {
                return;
            }
            Queue<MailSendInfo> mailQuere = new Queue<MailSendInfo>();
            var i = 0;
            while (i < mails.SendInfo.Count)
            {
                mailQuere.Enqueue(mails.SendInfo[0]);
                SendMail(new Mail()
                {
                        Host = mails.Host,
                        Port = mails.Port,
                        UserName = mails.UserName,
                        UserPassword = mails.UserPassword,
                        IsBodyHtml = mails.IsBodyHtml,
                        EnableSSL = mails.EnableSSL,
                        EmailToAddress = mails.SendInfo[i].EmailToAddress,
                        EmailToAddressCC = mails.SendInfo[i].EmailToAddressCC,
                        Body = mails.SendInfo[i].Body,
                        Subject = mails.SendInfo[i].Subject,
                      
                    
                });
                i++;
            }
        }
        public static string SendMail(Mail mail)
        {
            SmtpClient smtpClient = new SmtpClient(mail.Host, mail.Port);
            smtpClient.EnableSsl = mail.EnableSSL;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mail.UserName, mail.UserPassword);
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(mail.UserName);
                message.Subject = mail.Subject == null ? "" : mail.Subject;
                message.Body = mail.Body == null ? "" : mail.Body;
                message.IsBodyHtml = mail.IsBodyHtml;
                foreach (string email in mail.EmailToAddress)
                {
                    message.To.Add(email);
                }
                if (mail.EmailToAddressCC != null && mail.EmailToAddressCC.Length > 0)
                {
                    foreach (string emailCc in mail.EmailToAddressCC)
                    {
                        if (!string.IsNullOrEmpty(emailCc))
                            message.CC.Add(emailCc);
                    }
                }
                try
                {
                    smtpClient.Send(message);
                    return "Email Send SuccessFully";
                }
                catch
                {
                    return "Email Send failed";
                }
            }
        }
    }
}
