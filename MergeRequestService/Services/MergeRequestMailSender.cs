using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MergeRequestService.Models;

namespace MergeRequestService.Services
{
    public class MergeRequestMailSender : IMergeRequestMailSender
    {
        private readonly MailServerConfig _mailServerConfig;

        public MergeRequestMailSender(MailServerConfig mailServerConfig)
        {
            _mailServerConfig = mailServerConfig;
        }

        public void Send(MergeRequestMail mail)
        {
            var client = new SmtpClient(_mailServerConfig.SmtpServer)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_mailServerConfig.UserName, _mailServerConfig.Password),
                Port = _mailServerConfig.Port,
                EnableSsl = _mailServerConfig.EnableSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_mailServerConfig.FromAddress)
            };
            mail.Receivers.ForEach(receiver => mailMessage.To.Add(receiver));
            mail.Ccs.ForEach(cc => mailMessage.CC.Add(cc));
            mailMessage.Body = mail.Content;
            mailMessage.Subject = mail.Subject;
            client.Send(mailMessage);
        }
    }
}