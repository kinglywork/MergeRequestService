﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MergeRequestService.Models
{
    public class MailSendingJob
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string CronExpression { get; set; }
    }
}
