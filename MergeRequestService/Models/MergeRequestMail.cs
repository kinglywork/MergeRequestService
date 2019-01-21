using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MergeRequestService.Models
{
    public class MergeRequestMail
    {
        public string Receiver { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string TimeStamp { get; set; }

        private const char AddressSpliter = ';';

        public List<string> Receivers => Split(Receiver);

        public List<string> Ccs => Split(Cc);

        private static List<string> Split(string addresses)
        {
            return string.IsNullOrEmpty(addresses) ? new List<string>() : addresses.Split(AddressSpliter).ToList();
        }
    }
}