using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MergeRequestService.Models
{
    public class MailMessageConfig
    {
        public string Receiver { get; set; }
        public string Cc { get; set; }
        public string SubjectTemplate { get; set; }
    }
}