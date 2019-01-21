using System;
using System.Collections.Generic;
using System.Text;
using MergeRequestService.Models;
using MergeRequestService.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MergeRequestService.Test
{
    [TestClass]
    public class MergeRequestMailSenderTest
    {
//        [TestMethod] need valid config to run
        public void TestSendingMail()
        {
            //if use gmail smtp, turn on this setting to allow sending mail by app
            //https://myaccount.google.com/lesssecureapps
            var mailServerConfig = new MailServerConfig
            {
                FromAddress = "fromme@gmail.com",
                UserName = "fromme@gmail.com",
                Password = "MailAccountPassword",
                SmtpServer = "smtp.gmail.com"
            };
            var mailSender = new MergeRequestMailSender(mailServerConfig);
            var mail = new MergeRequestMail
            {
                Receiver = "receiver1@gmail.com;receiver2@yahoo.com",
                Cc = "cc1@gmail.com;cc2@yahoo.com",
                Content = "test content",
                Subject = "test subject",
                TimeStamp = DateTime.Now.ToString("MM/dd/yyyy")
            };
            mailSender.Send(mail);
        }
    }
}
