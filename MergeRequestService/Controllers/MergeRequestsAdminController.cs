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
    [Authorize]// todo admin only
    public class MergeRequestsAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MergeRequestsAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MergeRequestsAdmin
        public async Task<IActionResult> Index()
        {
            return View(await _context.MergeRequests.ToListAsync());
        }

        // GET: MergeRequestsAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mergeRequest = await _context.MergeRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mergeRequest == null)
            {
                return NotFound();
            }

            return View(mergeRequest);
        }

        // GET: MergeRequestsAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MergeRequestsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SourceBranch,TargetBranch,ChangeSetId,DevChangeSetId,Reviewer,Memo,SubmitBy,SubmitAt,UpdateBy,UpdateAt")] MergeRequest mergeRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mergeRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mergeRequest);
        }

        // GET: MergeRequestsAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mergeRequest = await _context.MergeRequests.FindAsync(id);
            if (mergeRequest == null)
            {
                return NotFound();
            }
            return View(mergeRequest);
        }

        // POST: MergeRequestsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SourceBranch,TargetBranch,ChangeSetId,DevChangeSetId,Reviewer,Memo,SubmitBy,SubmitAt,UpdateBy,UpdateAt")] MergeRequest mergeRequest)
        {
            if (id != mergeRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mergeRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MergeRequestExists(mergeRequest.Id))
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
            return View(mergeRequest);
        }

        // GET: MergeRequestsAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mergeRequest = await _context.MergeRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mergeRequest == null)
            {
                return NotFound();
            }

            return View(mergeRequest);
        }

        // POST: MergeRequestsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mergeRequest = await _context.MergeRequests.FindAsync(id);
            _context.MergeRequests.Remove(mergeRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MergeRequestExists(int id)
        {
            return _context.MergeRequests.Any(e => e.Id == id);
        }
    }
}
