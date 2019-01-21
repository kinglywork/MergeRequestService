using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MergeRequestService.Models
{
    public class MailServerConfig
    {
        public string SmtpServer { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public int Port { get;set; }
        public bool EnableSsl { get;set; }
    }
}