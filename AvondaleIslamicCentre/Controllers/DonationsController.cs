using AvondaleIslamicCentre.Areas.Identity.Data;
using AvondaleIslamicCentre.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AvondaleIslamicCentre.Controllers
{
    // Only logged-in users can access this controller
    [Authorize]
    public class DonationsController : Controller
    {
        private readonly AICDbContext _context;
        private const int PageSize = 10; // Controls how many donations show per page
        private readonly UserManager<AICUser> _userManager;

        // Constructor sets up the database context and user manager
        public DonationsController(AICDbContext context, UserManager<AICUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Show a paginated list of donations with sorting and search options
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            // Store sorting and search info for the view
            ViewData["CurrentSort"] = sortOrder;
            ViewData["AmountSortParm"] = String.IsNullOrEmpty(sortOrder) ? "amount_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["CurrentFilter"] = searchString;

            // Load all donations, including user info
            var donations = _context.Donations.Include(d => d.AICUser).AsQueryable();

            // Members can only see their own donations
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                donations = donations.Where(d => d.AICUserId == currentUserId);
            }

            // Apply search filtering if a search term is provided
            if (!String.IsNullOrEmpty(searchString))
            {
                var s = searchString.Trim();

                // Match donation description text
                IQueryable<Donation> filtered = donations.Where(d => d.Description != null && d.Description.Contains(s));

                // Try matching donation type if the search term matches an enum value
                if (Enum.TryParse<DonationType>(s, true, out var dt))
                {
                    filtered = filtered.Union(donations.Where(d => d.DonationType == dt));
                }

                // Try matching payment method if the search term matches an enum value
                if (Enum.TryParse<PaymentMethod>(s, true, out var pm))
                {
                    filtered = filtered.Union(donations.Where(d => d.PaymentMethod == pm));
                }

                // Search by donor user ID or their first/last name
                filtered = filtered.Union(donations.Where(d => d.AICUserId != null && d.AICUserId.Contains(s)));
                filtered = filtered.Union(donations.Where(d => d.AICUser != null &&
                    (d.AICUser.FirstName.Contains(s) || d.AICUser.LastName.Contains(s))));

                donations = filtered;
            }

            // Sort donations based on amount or date
            donations = sortOrder switch
            {
                "amount_desc" => donations.OrderByDescending(d => d.Amount),
                "date" => donations.OrderBy(d => d.DateDonated),
                "date_desc" => donations.OrderByDescending(d => d.DateDonated),
                _ => donations.OrderBy(d => d.Amount),
            };

            // Return the paginated list of donations to the view
            return View(await PaginatedList<Donation>.CreateAsync(donations.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        // Show details of a specific donation
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load donation with user info
            var donation = await _context.Donations
                .Include(d => d.AICUser)
                .FirstOrDefaultAsync(m => m.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }

            // Members can only view their own donation details
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                if (donation.AICUserId != currentUserId)
                {
                    return Forbid();
                }
            }

            return View(donation);
        }

        // Display form for creating a new donation
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Create()
        {
            // Pass the current user's info to the view
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserId = _userManager.GetUserId(User);
            ViewBag.CurrentUserFirstName = currentUser?.FirstName ?? currentUser?.UserName ?? "";

            return View();
        }

        // Save a new donation to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Create([Bind("DonationId,Amount,DonationType,PaymentMethod,Description")] Donation donation)
        {
            // Set the donation date to now and assign ownership to the current user
            donation.DateDonated = DateTime.Now;
            donation.AICUserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                _context.Add(donation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, reload the form with user info
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserId = _userManager.GetUserId(User);
            ViewBag.CurrentUserFirstName = currentUser?.FirstName ?? currentUser?.UserName ?? "";
            return View(donation);
        }

        // Display form for editing an existing donation
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load the donation with related user info
            var donation = await _context.Donations.Include(d => d.AICUser)
                .FirstOrDefaultAsync(d => d.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }

            // Members can only edit their own donations
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                if (donation.AICUserId != currentUserId)
                {
                    return Forbid();
                }
            }

            // Send the donor’s name and ID to the view
            ViewBag.DonationOwnerFirstName = donation.AICUser?.FirstName ??
                (await _userManager.FindByIdAsync(donation.AICUserId))?.FirstName ?? "";
            ViewBag.DonationOwnerId = donation.AICUserId;

            return View(donation);
        }

        // Save the edited donation
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Edit(int id, [Bind("DonationId,Amount,DonationType,PaymentMethod,Description")] Donation donation)
        {
            if (id != donation.DonationId)
            {
                return NotFound();
            }

            // Load the existing donation to preserve unchanged fields
            var existing = await _context.Donations.AsNoTracking()
                .FirstOrDefaultAsync(d => d.DonationId == id);
            if (existing == null) return NotFound();

            // Keep the original date and owner info
            donation.DateDonated = existing.DateDonated;
            donation.AICUserId = existing.AICUserId;

            // Members cannot edit other users' donations
            if (User.IsInRole("Member") && existing.AICUserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle case where the donation might have been deleted
                    if (!DonationExists(donation.DonationId))
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

            // If validation fails, reload the form with donor info
            ViewBag.DonationOwnerFirstName = (await _userManager.FindByIdAsync(donation.AICUserId))?.FirstName ?? "";
            ViewBag.DonationOwnerId = donation.AICUserId;
            return View(donation);
        }

        // Show confirmation page before deleting a donation
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load donation and related user info
            var donation = await _context.Donations
                .Include(d => d.AICUser)
                .FirstOrDefaultAsync(m => m.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }

            // Members can only delete their own donations
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                if (donation.AICUserId != currentUserId)
                {
                    return Forbid();
                }
            }

            return View(donation);
        }

        // Permanently delete a donation after confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Load existing donation to check permissions
            var existing = await _context.Donations.AsNoTracking()
                .FirstOrDefaultAsync(d => d.DonationId == id);
            if (existing == null) return NotFound();

            // Prevent members from deleting someone else’s donation
            if (User.IsInRole("Member") && existing.AICUserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            // Delete the donation if found
            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
            {
                _context.Donations.Remove(donation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Check if a donation exists by its ID
        private bool DonationExists(int id)
        {
            return _context.Donations.Any(e => e.DonationId == id);
        }
    }
}
