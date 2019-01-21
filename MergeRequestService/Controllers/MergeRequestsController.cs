using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MergeRequestService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MergeRequestService.Models;
using MergeRequestService.Services;
using Microsoft.AspNetCore.Authorization;

namespace MergeRequestService.Controllers
{
    [Authorize]
    public class MergeRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITargetBranchListFactory _targetBranchListFactory;

        private DateTime Now => DateTime.Now;
        private string LoginUser => User.Identity.Name;

        public MergeRequestsController(ApplicationDbContext context,
            ITargetBranchListFactory targetBranchListFactory)
        {
            _context = context;
            _targetBranchListFactory = targetBranchListFactory;
        }

        // GET: MergeRequests
        public async Task<IActionResult> Index()
        {
            var dateTimeStart = Now.Date;
            var dateTimeEnd = Now.Date.AddDays(1);
            var loginUser = LoginUser;
            var mergeRequests = await _context.MergeRequests
                .Where(r => r.SubmitAt >= dateTimeStart
                            && r.SubmitAt < dateTimeEnd
                            && r.SubmitBy == loginUser)
                .ToListAsync();

            return View(mergeRequests);
        }

        // GET: MergeRequests/Details/5
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

        // GET: MergeRequests/Create
        public IActionResult Create()
        {
            ViewData["TargetBranches"] = _targetBranchListFactory.Create();
            return View();
        }

        // POST: MergeRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MergeRequest mergeRequest)
        {
            if (ModelState.IsValid)
            {
                mergeRequest.SubmitAt = Now;
                mergeRequest.SubmitBy = LoginUser;
                mergeRequest.UpdateAt = Now;
                mergeRequest.UpdateBy = LoginUser;
                _context.Add(mergeRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(mergeRequest);
        }

        // GET: MergeRequests/Edit/5
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

        // POST: MergeRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SourceBranch,TargetBranch,ChangeSetId,IsToQa,DevChangeSetId,Memo")]
            MergeRequest mergeRequest)
        {
            if (id != mergeRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!AuthorizeAccess(mergeRequest))
                {
                    return Forbid();
                }

                try
                {
                    mergeRequest.UpdateAt = Now;
                    mergeRequest.UpdateBy = LoginUser;
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

        // GET: MergeRequests/Delete/5
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

        // POST: MergeRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mergeRequest = await _context.MergeRequests.FindAsync(id);
            if (!AuthorizeAccess(mergeRequest))
            {
                return Forbid();
            }

            _context.MergeRequests.Remove(mergeRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MergeRequestExists(int id)
        {
            return _context.MergeRequests.Any(e => e.Id == id);
        }

        private bool AuthorizeAccess(MergeRequest mergeRequest)
        {
            return mergeRequest.SubmitBy == LoginUser;
        }
    }
}