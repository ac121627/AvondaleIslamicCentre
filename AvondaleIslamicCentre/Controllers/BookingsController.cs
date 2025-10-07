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
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly AICDbContext _context;
        private const int PageSize = 10;

        public BookingsController(AICDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["StartSortParm"] = String.IsNullOrEmpty(sortOrder) ? "start_desc" : "";
            ViewData["EndSortParm"] = sortOrder == "end" ? "end_desc" : "end";
            ViewData["CurrentFilter"] = searchString;

            var bookings = _context.Booking.Include(b => b.AICUser).Include(b => b.Hall).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b => (b.AICUserId != null && b.AICUserId.Contains(searchString)) || b.Hall.Name.Contains(searchString));
            }

            bookings = sortOrder switch
            {
                "start_desc" => bookings.OrderByDescending(b => b.StartDateTime),
                "end" => bookings.OrderBy(b => b.EndDateTime),
                "end_desc" => bookings.OrderByDescending(b => b.EndDateTime),
                _ => bookings.OrderBy(b => b.StartDateTime),
            };

            return View(await PaginatedList<Booking>.CreateAsync(bookings.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.AICUser)
                .Include(b => b.Hall)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        [Authorize(Roles = "Admin,Member")]
        public IActionResult Create()
        {
            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "HallId", "Name");
            ViewData["AICUserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Create([Bind("BookingId,StartDateTime,EndDateTime,HallId,AICUserId")] Booking booking)
        {
            // If no AICUserId supplied, use current user
            if (string.IsNullOrWhiteSpace(booking.AICUserId))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                booking.AICUserId = userId ?? string.Empty;
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "HallId", "Name", booking.HallId);
            ViewData["AICUserId"] = new SelectList(_context.Users, "Id", "UserName", booking.AICUserId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "HallId", "Name", booking.HallId);
            ViewData["AICUserId"] = new SelectList(_context.Users, "Id", "UserName", booking.AICUserId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,StartDateTime,EndDateTime,HallId,AICUserId")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "HallId", "Name", booking.HallId);
            ViewData["AICUserId"] = new SelectList(_context.Users, "Id", "UserName", booking.AICUserId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.AICUser)
                .Include(b => b.Hall)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }
    }
}
