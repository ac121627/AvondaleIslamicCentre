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
    [Authorize]
    public class DonationsController : Controller
    {
        private readonly AICDbContext _context;
        private const int PageSize = 10;
        private readonly UserManager<AICUser> _userManager;

        public DonationsController(AICDbContext context, UserManager<AICUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Donations
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["AmountSortParm"] = String.IsNullOrEmpty(sortOrder) ? "amount_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["CurrentFilter"] = searchString;

            var donations = _context.Donations.Include(d => d.AICUser).AsQueryable();

            // If user is a Member, only show their own donations
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                donations = donations.Where(d => d.AICUserId == currentUserId);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                var s = searchString.Trim();

                // match description text
                IQueryable<Donation> filtered = donations.Where(d => d.Description != null && d.Description.Contains(s));

                // try parse DonationType
                if (Enum.TryParse<DonationType>(s, true, out var dt))
                {
                    filtered = filtered.Union(donations.Where(d => d.DonationType == dt));
                }

                // try parse PaymentMethod
                if (Enum.TryParse<PaymentMethod>(s, true, out var pm))
                {
                    filtered = filtered.Union(donations.Where(d => d.PaymentMethod == pm));
                }

                donations = filtered;
            }

            donations = sortOrder switch
            {
                "amount_desc" => donations.OrderByDescending(d => d.Amount),
                "date" => donations.OrderBy(d => d.DateDonated),
                "date_desc" => donations.OrderByDescending(d => d.DateDonated),
                _ => donations.OrderBy(d => d.Amount),
            };

            return View(await PaginatedList<Donation>.CreateAsync(donations.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        // GET: Donations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.AICUser)
                .FirstOrDefaultAsync(m => m.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }

            // Members may only view their own donation details
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

        // GET: Donations/Create
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Create()
        {
            // populate AICUser select for frontend
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserId = _userManager.GetUserId(User);
            ViewBag.CurrentUserFirstName = currentUser?.FirstName ?? currentUser?.UserName ?? "";

            return View();
        }

        // POST: Donations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Create([Bind("DonationId,Amount,DonationType,PaymentMethod,Description")] Donation donation)
        {
            // Force DateDonated to now and owner to current user
            donation.DateDonated = DateTime.Now;
            donation.AICUserId = _userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                _context.Add(donation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserId = _userManager.GetUserId(User);
            ViewBag.CurrentUserFirstName = currentUser?.FirstName ?? currentUser?.UserName ?? "";
            return View(donation);
        }

        // GET: Donations/Edit/5
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.Include(d => d.AICUser).FirstOrDefaultAsync(d => d.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }

            // Members may only edit their own donations
            if (User.IsInRole("Member"))
            {
                var currentUserId = _userManager.GetUserId(User);
                if (donation.AICUserId != currentUserId)
                {
                    return Forbid();
                }
            }

            ViewBag.DonationOwnerFirstName = donation.AICUser?.FirstName ?? (await _userManager.FindByIdAsync(donation.AICUserId))?.FirstName ?? "";
            ViewBag.DonationOwnerId = donation.AICUserId;

            return View(donation);
        }

        // POST: Donations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Edit(int id, [Bind("DonationId,Amount,DonationType,PaymentMethod,Description")] Donation donation)
        {
            if (id != donation.DonationId)
            {
                return NotFound();
            }

            var existing = await _context.Donations.AsNoTracking().FirstOrDefaultAsync(d => d.DonationId == id);
            if (existing == null) return NotFound();

            // Preserve fields that cannot be changed
            donation.DateDonated = existing.DateDonated;
            donation.AICUserId = existing.AICUserId;

            // Members cannot change owner; check ownership
            if (User.IsInRole("Member") && existing.AICUserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
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

            ViewBag.DonationOwnerFirstName = (await _userManager.FindByIdAsync(donation.AICUserId))?.FirstName ?? "";
            ViewBag.DonationOwnerId = donation.AICUserId;
            return View(donation);
        }

        // GET: Donations/Delete/5
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.AICUser)
                .FirstOrDefaultAsync(m => m.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }

            // Members may only delete their own donations
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

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var existing = await _context.Donations.AsNoTracking().FirstOrDefaultAsync(d => d.DonationId == id);
            if (existing == null) return NotFound();

            if (User.IsInRole("Member") && existing.AICUserId != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
            {
                _context.Donations.Remove(donation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonationExists(int id)
        {
            return _context.Donations.Any(e => e.DonationId == id);
        }
    }
}
