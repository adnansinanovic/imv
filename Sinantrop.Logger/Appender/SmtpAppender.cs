using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Sinantrop.Logger.Appender
{
    public class SmtpAppender : BaseAppender
    {
        public SmtpAppender()
        {
            UseSeparateThread = true;
        }

        public MailAddress FromAddress { get; set; }
        public MailAddress ToAddress { get; set; }
        public string FromPassword { get; set; }
        public string Subject { get; set; }
        public string Host { get; set; }

        public int Port { get; set; }

        public bool EnableSsl { get; set; }

        public SmtpDeliveryMethod DeliveryMethod { get; set; }

        public bool UseDefaultCredentials { get; set; }
       
        public override void Append(LogEvent logEvent)
        {
            var smtp = new SmtpClient
            {
                Host = Host,
                Port = Port,
                EnableSsl = true,
                DeliveryMethod = DeliveryMethod,
                UseDefaultCredentials = UseDefaultCredentials,
                Credentials = new NetworkCredential(FromAddress.Address, FromPassword)
            };

            using (StringWriter writer = new StringWriter())
            {
                Layout.Format(writer, logEvent);

                using (var message = new MailMessage(FromAddress, ToAddress) { Subject = Subject, Body = writer.ToString() })
                {
                    try
                    {
                        smtp.Send(message);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.ToString());
                    }                   
                }
            }
        }
    }

    public class GmailSmtpAppender : SmtpAppender
    {                
        public GmailSmtpAppender(MailAddress fromAddress, MailAddress toAddress, string fromPassword, string subject)
        {
            FromAddress = fromAddress;
            ToAddress = toAddress;
            FromPassword = fromPassword;
            Subject = subject;

            Host = "smtp.gmail.com";
            Port = 587;
            EnableSsl = false;
            DeliveryMethod = SmtpDeliveryMethod.Network;
            UseDefaultCredentials = false;                
        }
    }
}
