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
using Microsoft.AspNetCore.Identity;

namespace AvondaleIslamicCentre.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly AICDbContext _context;
        private const int PageSize = 10;
        private readonly UserManager<AICUser> _userManager;

        public BookingsController(AICDbContext context, UserManager<AICUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["StartSortParm"] = String.IsNullOrEmpty(sortOrder) ? "start_desc" : "";
            ViewData["EndSortParm"] = sortOrder == "end" ? "end_desc" : "end";
            ViewData["CurrentFilter"] = searchString;

            var bookings = _context.Booking.Include(b => b.AICUser).Include(b => b.Hall).AsQueryable();

            // If user is a Member, only show their own bookings
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                bookings = bookings.Where(b => b.AICUserId == currentUserId);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                var s = searchString.Trim();
                // search by hall name
                IQueryable<Booking> filtered = bookings.Where(b => b.Hall.Name.Contains(s));

                // search by user id (username) or user's first/last name
                filtered = filtered.Union(bookings.Where(b => b.AICUserId != null && b.AICUserId.Contains(s)));
                filtered = filtered.Union(bookings.Where(b => b.AICUser != null && (b.AICUser.FirstName.Contains(s) || b.AICUser.LastName.Contains(s))));

                bookings = filtered;
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

            // Members may only view their own booking details
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                if (booking.AICUserId != currentUserId)
                {
                    return Forbid();
                }
            }

            return View(booking);
        }

        // GET: Bookings/Create
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Create()
        {
            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "HallId", "Name");

            // Provide current user id and first name to the view
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserId = _userManager.GetUserId(User);
            ViewBag.CurrentUserFirstName = currentUser?.FirstName ?? currentUser?.UserName ?? "";

            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Create([Bind("BookingId,StartDateTime,EndDateTime,HallId")] Booking booking)
        {
            // Force owner to current user
            booking.AICUserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "HallId", "Name", booking.HallId);
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserId = _userManager.GetUserId(User);
            ViewBag.CurrentUserFirstName = currentUser?.FirstName ?? currentUser?.UserName ?? "";
            return View(booking);
        }

        // GET: Bookings/Edit/5
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.Include(b => b.AICUser).FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            // Members may only edit their own bookings
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                if (booking.AICUserId != currentUserId)
                {
                    return Forbid();
                }
            }

            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "HallId", "Name", booking.HallId);
            ViewBag.BookingOwnerFirstName = booking.AICUser?.FirstName ?? (await _userManager.FindByIdAsync(booking.AICUserId))?.FirstName ?? "";
            ViewBag.BookingOwnerId = booking.AICUserId;
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,StartDateTime,EndDateTime,HallId")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            // Retrieve existing booking to validate ownership and preserve AICUserId
            var existing = await _context.Booking.AsNoTracking().FirstOrDefaultAsync(b => b.BookingId == id);
            if (existing == null) return NotFound();

            // If user is Member, ensure they own the booking and prevent changing owner
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                if (existing.AICUserId != currentUserId)
                {
                    return Forbid();
                }
                // Force owner to remain the current user
                booking.AICUserId = currentUserId;
            }
            else
            {
                // For Admins, keep existing owner (do not allow change from UI)
                booking.AICUserId = existing.AICUserId;
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
            var owner = await _userManager.FindByIdAsync(booking.AICUserId);
            ViewBag.BookingOwnerFirstName = owner?.FirstName ?? "";
            ViewBag.BookingOwnerId = booking.AICUserId;
            return View(booking);
        }

        // GET: Bookings/Delete/5
        [Authorize(Roles = "Admin,Member")]
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

            // Members may only delete their own bookings
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                if (booking.AICUserId != currentUserId)
                {
                    return Forbid();
                }
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var existing = await _context.Booking.AsNoTracking().FirstOrDefaultAsync(b => b.BookingId == id);
            if (existing == null) return NotFound();

            if (User.IsInRole("Member") && existing.AICUserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

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
