using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MergeRequestService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MergeRequestService.Models;
using Microsoft.AspNetCore.Authorization;

namespace MergeRequestService.Controllers
{
    [Authorize]//todo admin only
    public class MailSendingJobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MailSendingJobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MailSendingJobs
        public async Task<IActionResult> Index()
        {
            return View(await _context.MailSendingJobs.ToListAsync());
        }

        // GET: MailSendingJobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mailSendingJob = await _context.MailSendingJobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mailSendingJob == null)
            {
                return NotFound();
            }

            return View(mailSendingJob);
        }

        // GET: MailSendingJobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MailSendingJobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Active,CronExpression")] MailSendingJob mailSendingJob)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mailSendingJob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mailSendingJob);
        }

        // GET: MailSendingJobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mailSendingJob = await _context.MailSendingJobs.FindAsync(id);
            if (mailSendingJob == null)
            {
                return NotFound();
            }
            return View(mailSendingJob);
        }

        // POST: MailSendingJobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Active,CronExpression")] MailSendingJob mailSendingJob)
        {
            if (id != mailSendingJob.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mailSendingJob);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MailSendingJobExists(mailSendingJob.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mailSendingJob);
        }

        // GET: MailSendingJobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mailSendingJob = await _context.MailSendingJobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mailSendingJob == null)
            {
                return NotFound();
            }

            return View(mailSendingJob);
        }

        // POST: MailSendingJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mailSendingJob = await _context.MailSendingJobs.FindAsync(id);
            _context.MailSendingJobs.Remove(mailSendingJob);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MailSendingJobExists(int id)
        {
            return _context.MailSendingJobs.Any(e => e.Id == id);
        }
    }
}
