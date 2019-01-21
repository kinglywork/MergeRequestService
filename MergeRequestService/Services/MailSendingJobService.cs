using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MergeRequestService.Data;
using MergeRequestService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MergeRequestService.Services
{
    public class MailSendingJobService : IMailSendingJobService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOptions<MailServerConfig> _mailServerConfig;
        private readonly IOptions<MailMessageConfig> _mailMessageConfig;
        private readonly IMergeRequestMailGenerator _mergeRequestMailGenerator;

        private DateTime Now => DateTime.Now;

        public MailSendingJobService(ApplicationDbContext context,
            IOptions<MailServerConfig> mailServerConfig,
            IOptions<MailMessageConfig> mailMessageConfig,
            IMergeRequestMailGenerator mergeRequestMailGenerator)
        {
            _context = context;
            _mailServerConfig = mailServerConfig;
            _mailMessageConfig = mailMessageConfig;
            _mergeRequestMailGenerator = mergeRequestMailGenerator;
        }

        public void SendTodayMergeRequestMail()
        {
            var mergeRequests = _context.MergeRequests.Where(r => r.SubmitAt < Now.AddDays(1).Date && r.SubmitAt >= Now.Date).ToList();
            var mergeRequestsContent = _mergeRequestMailGenerator.GenerateMergeRequests(mergeRequests);
            var mailBody = _mergeRequestMailGenerator.GenerateMailBody(mergeRequestsContent);


            var mailSender = new MergeRequestMailSender(_mailServerConfig.Value);
            var mail = new MergeRequestMail
            {
                Receiver = _mailMessageConfig.Value.Receiver,
                Cc = _mailMessageConfig.Value.Cc,
                Content = mailBody,
                Subject = string.Format(_mailMessageConfig.Value.SubjectTemplate, Now),
                TimeStamp = Now.ToString("MM/dd/yyyy")
            };
            mailSender.Send(mail);
        }
    }
}