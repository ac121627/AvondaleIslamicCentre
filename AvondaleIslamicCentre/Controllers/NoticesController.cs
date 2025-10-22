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
        private const int PageSize = 10; // Controls how many notices show per page

        // Constructor to set up the database context
        public NoticesController(AICDbContext context)
        {
            _context = context;
        }

        // Show a list of all notices with sorting, searching, and pagination
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            // Keep track of sorting and search settings for the view
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            // Load all notices and include the user who posted them
            var notices = _context.Notices.Include(n => n.AICUser).AsQueryable();

            // If a search term is entered, filter notices by title or message content
            if (!String.IsNullOrEmpty(searchString))
            {
                notices = notices.Where(n =>
                    (n.Title != null && n.Title.Contains(searchString)) ||
                    (n.Message != null && n.Message.Contains(searchString)));
            }

            // Sort the notices by posted date
            notices = sortOrder switch
            {
                "date_desc" => notices.OrderByDescending(n => n.PostedAt),
                _ => notices.OrderBy(n => n.PostedAt),
            };

            // Return paginated results to the view
            return View(await PaginatedList<Notice>.CreateAsync(notices.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        // Show the details of a specific notice
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the notice by its ID and include the user who posted it
            var notice = await _context.Notices
                .Include(n => n.AICUser)
                .FirstOrDefaultAsync(m => m.NoticeId == id);
            if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
        }

        // Display form to create a new notice (Admins only)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Save a new notice to the database (Admins only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("NoticeId,Title,Message,UpdatedAt")] Notice notice)
        {
            // Validate and save the new notice
            if (ModelState.IsValid)
            {
                // Set the post time and assign the notice to the current logged-in user
                notice.PostedAt = DateTime.Now;
                notice.AICUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

                _context.Add(notice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If something goes wrong, show the form again
            return View(notice);
        }

        // Display form to edit an existing notice (Admins only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the notice by its ID
            var notice = await _context.Notices.FindAsync(id);
            if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
        }

        // Save the edited notice (Admins only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("NoticeId,Title,Message,PostedAt,UpdatedAt")] Notice notice)
        {
            if (id != notice.NoticeId)
            {
                return NotFound();
            }

            // Update the notice if the data is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Preserve the original author
                    var existing = await _context.Notices.AsNoTracking().FirstOrDefaultAsync(n => n.NoticeId == id);
                    if (existing == null) return NotFound();
                    notice.AICUserId = existing.AICUserId;

                    _context.Update(notice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle case where the notice no longer exists
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

            // If validation fails, reload the form
            return View(notice);
        }

        // Display confirmation page before deleting a notice (Admins only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load the notice and the user who created it
            var notice = await _context.Notices
                .Include(n => n.AICUser)
                .FirstOrDefaultAsync(m => m.NoticeId == id);
            if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
        }

        // Permanently delete a notice after confirmation (Admins only)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the notice and remove it if found
            var notice = await _context.Notices.FindAsync(id);
            if (notice != null)
            {
                _context.Notices.Remove(notice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Check if a notice exists in the database by its ID
        private bool NoticeExists(int id)
        {
            return _context.Notices.Any(e => e.NoticeId == id);
        }
    }
}
