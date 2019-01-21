using System;
using System.Linq;
using MergeRequestService.Data;
using MergeRequestService.Models;
using MergeRequestService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MergeRequestService.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class MergeRequestMailPreviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOptions<MailMessageConfig> _mailMessageConfig;
        private readonly IMergeRequestMailGenerator _mergeRequestMailGenerator;

        private DateTime Now => DateTime.Now;

        public MergeRequestMailPreviewController(ApplicationDbContext context,
            IOptions<MailMessageConfig> mailMessageConfig,
            IMergeRequestMailGenerator mergeRequestMailGenerator)
        {
            _context = context;
            _mailMessageConfig = mailMessageConfig;
            _mergeRequestMailGenerator = mergeRequestMailGenerator;
        }

        public IActionResult Index()
        {
            var mergeRequests = _context.MergeRequests.Where(r => r.SubmitAt < Now.AddDays(1).Date && r.SubmitAt >= Now.Date).ToList();
            var mergeRequestsContent = _mergeRequestMailGenerator.GenerateMergeRequests(mergeRequests);
            var mailBody = _mergeRequestMailGenerator.GenerateMailBody(mergeRequestsContent);

            var mail = new MergeRequestMail
            {
                Receiver = _mailMessageConfig.Value.Receiver,
                Cc = _mailMessageConfig.Value.Cc,
                Subject = string.Format(_mailMessageConfig.Value.SubjectTemplate, Now),
                Content = mailBody
            };

            return View(mail);
        }
    }
}