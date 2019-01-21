using MergeRequestService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MergeRequestService.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MergeRequest> MergeRequests { get; set; }

        public DbSet<MailSendingJob> MailSendingJobs { get; set; }
    }
}
