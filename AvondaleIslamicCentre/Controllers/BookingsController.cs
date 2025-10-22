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
    // Only logged-in users can access this controller
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly AICDbContext _context;
        private const int PageSize = 10; // Controls how many bookings show per page
        private readonly UserManager<AICUser> _userManager;

        // Constructor to set up database and user manager
        public BookingsController(AICDbContext context, UserManager<AICUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Show a paginated list of bookings, with search and sorting options
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            // Set up sorting and search options for the view
            ViewData["CurrentSort"] = sortOrder;
            ViewData["StartSortParm"] = String.IsNullOrEmpty(sortOrder) ? "start_desc" : "";
            ViewData["EndSortParm"] = sortOrder == "end" ? "end_desc" : "end";
            ViewData["CurrentFilter"] = searchString;

            // Start with all bookings, including user and hall details
            var bookings = _context.Booking.Include(b => b.AICUser).Include(b => b.Hall).AsQueryable();

            // Members can only see their own bookings
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                bookings = bookings.Where(b => b.AICUserId == currentUserId);
            }

            // If a search term is entered, filter results
            if (!String.IsNullOrEmpty(searchString))
            {
                var s = searchString.Trim();

                // Search by hall name
                IQueryable<Booking> filtered = bookings.Where(b => b.Hall.Name.Contains(s));

                // Search by user ID, first name, or last name
                filtered = filtered.Union(bookings.Where(b => b.AICUserId != null && b.AICUserId.Contains(s)));
                filtered = filtered.Union(bookings.Where(b => b.AICUser != null && (b.AICUser.FirstName.Contains(s) || b.AICUser.LastName.Contains(s))));

                bookings = filtered;
            }

            // Sort results based on selected sort order
            bookings = sortOrder switch
            {
                "start_desc" => bookings.OrderByDescending(b => b.StartDateTime),
                "end" => bookings.OrderBy(b => b.EndDateTime),
                "end_desc" => bookings.OrderByDescending(b => b.EndDateTime),
                _ => bookings.OrderBy(b => b.StartDateTime),
            };

            // Return the paginated list to the view
            return View(await PaginatedList<Booking>.CreateAsync(bookings.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        // Show details for a specific booking
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get booking with user and hall info
            var booking = await _context.Booking
                .Include(b => b.AICUser)
                .Include(b => b.Hall)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            // Members can only view their own bookings
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

        // Display the form to create a new booking
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Create()
        {
            // List of halls for dropdown selection
            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "HallId", "Name");

            // Get info about the currently logged-in user
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserId = _userManager.GetUserId(User);
            ViewBag.CurrentUserFirstName = currentUser?.FirstName ?? currentUser?.UserName ?? "";

            return View();
        }

        // Save a new booking to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Create([Bind("BookingId,StartDateTime,EndDateTime,HallId")] Booking booking)
        {
            // Automatically assign the booking to the logged-in user
            booking.AICUserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If something goes wrong, re-display the form
            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "HallId", "Name", booking.HallId);
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserId = _userManager.GetUserId(User);
            ViewBag.CurrentUserFirstName = currentUser?.FirstName ?? currentUser?.UserName ?? "";
            return View(booking);
        }

        // Display the form to edit an existing booking
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load booking and user info
            var booking = await _context.Booking.Include(b => b.AICUser).FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            // Members can only edit their own bookings
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                if (booking.AICUserId != currentUserId)
                {
                    return Forbid();
                }
            }

            // Pass hall list and booking owner info to the view
            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "HallId", "Name", booking.HallId);
            ViewBag.BookingOwnerFirstName = booking.AICUser?.FirstName ?? (await _userManager.FindByIdAsync(booking.AICUserId))?.FirstName ?? "";
            ViewBag.BookingOwnerId = booking.AICUserId;
            return View(booking);
        }

        // Save the edited booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,StartDateTime,EndDateTime,HallId")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            // Load existing booking to check permissions
            var existing = await _context.Booking.AsNoTracking().FirstOrDefaultAsync(b => b.BookingId == id);
            if (existing == null) return NotFound();

            // Members can only update their own bookings
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                if (existing.AICUserId != currentUserId)
                {
                    return Forbid();
                }

                // Ensure the booking still belongs to the current user
                booking.AICUserId = currentUserId;
            }
            else
            {
                // Admins cannot change ownership
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

            // If validation fails, reload view data
            ViewData["HallId"] = new SelectList(_context.Set<Hall>(), "HallId", "Name", booking.HallId);
            var owner = await _userManager.FindByIdAsync(booking.AICUserId);
            ViewBag.BookingOwnerFirstName = owner?.FirstName ?? "";
            ViewBag.BookingOwnerId = booking.AICUserId;
            return View(booking);
        }

        // Display confirmation page for deleting a booking
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load booking with related data
            var booking = await _context.Booking
                .Include(b => b.AICUser)
                .Include(b => b.Hall)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            // Members can only delete their own bookings
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

        // Handle delete confirmation and remove booking from database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Load booking again to confirm permissions
            var existing = await _context.Booking.AsNoTracking().FirstOrDefaultAsync(b => b.BookingId == id);
            if (existing == null) return NotFound();

            // Prevent members from deleting someone else’s booking
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

        // Check if a booking exists by its ID
        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }
    }
}
