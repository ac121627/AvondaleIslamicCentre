using AvondaleIslamicCentre.Areas.Identity.Data;
using AvondaleIslamicCentre.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace AvondaleIslamicCentre.Controllers
{
    public class NoticesController : Controller
    {
        private readonly AICDbContext _context;
        private const int PageSize = 10;

        public NoticesController(AICDbContext context)
        {
            _context = context;
        }

        // GET: Notices
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            var notices = _context.Notices.Include(n => n.AICUser).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                notices = notices.Where(n => (n.Title != null && n.Title.Contains(searchString)) || (n.Message != null && n.Message.Contains(searchString)));
            }

            notices = sortOrder switch
            {
                "date_desc" => notices.OrderByDescending(n => n.PostedAt),
                _ => notices.OrderBy(n => n.PostedAt),
            };

            return View(await PaginatedList<Notice>.CreateAsync(notices.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        // GET: Notices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices
                .Include(n => n.AICUser)
                .FirstOrDefaultAsync(m => m.NoticeId == id);
            if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
        }

        // GET: Notices/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("NoticeId,Title,Message,UpdatedAt")] Notice notice)
        {
            if (ModelState.IsValid)
            {
                notice.PostedAt = DateTime.Now;
                notice.AICUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
                _context.Add(notice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notice);
        }

        // GET: Notices/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices.FindAsync(id);
            if (notice == null)
            {
                return NotFound();
            }
            return View(notice);
        }

        // POST: Notices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("NoticeId,Title,Message,PostedAt,UpdatedAt")] Notice notice)
        {
            if (id != notice.NoticeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Notices.AsNoTracking().FirstOrDefaultAsync(n => n.NoticeId == id);
                    if (existing == null) return NotFound();
                    notice.AICUserId = existing.AICUserId;
                    notice.UpdatedAt = DateTime.Now;
                    _context.Update(notice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticeExists(notice.NoticeId))
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
            return View(notice);
        }

        // GET: Notices/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices
                .Include(n => n.AICUser)
                .FirstOrDefaultAsync(m => m.NoticeId == id);
            if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
        }

        // POST: Notices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notice = await _context.Notices.FindAsync(id);
            if (notice != null)
            {
                _context.Notices.Remove(notice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoticeExists(int id)
        {
            return _context.Notices.Any(e => e.NoticeId == id);
        }
    }
}
