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

            if (!String.IsNullOrEmpty(searchString))
            {
                donations = donations.Where(d => (d.DonorName != null && d.DonorName.Contains(searchString)) || (d.Description != null && d.Description.Contains(searchString)));
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

        /*[Authorize]

        public async Task<IActionResult> ViewDonations()
        {
            var currentUserId = _userManager.GetUserId(User);

            var donations = await _context.Donations
            .Where(f => f.AICUserId == currentUserId)
            .OrderByDescending(f => f.Amount)
            .ToListAsync();

            return View(donations);
        }*/

        // GET: Donations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Members should not see donation details
            if (User.IsInRole("Member"))
            {
                return Forbid();
            }

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

            return View(donation);
        }

        // GET: Donations/Create
        [Authorize(Roles = "Admin,Member")]
        public IActionResult Create()
        {
            // populate AICUser select for frontend
            ViewData["AICUserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Donations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Member")]
        public async Task<IActionResult> Create([Bind("DonationId,Amount,DateDonated,DonorName,DonationType,PaymentMethod,Description,AICUserId")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AICUserId"] = new SelectList(_context.Users, "Id", "UserName", donation.AICUserId);
            return View(donation);
        }

        // GET: Donations/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            ViewData["AICUserId"] = new SelectList(_context.Users, "Id", "UserName", donation.AICUserId);
            return View(donation);
        }

        // POST: Donations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("DonationId,Amount,DateDonated,DonorName,DonationType,PaymentMethod,Description,AICUserId")] Donation donation)
        {
            if (id != donation.DonationId)
            {
                return NotFound();
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
            ViewData["AICUserId"] = new SelectList(_context.Users, "Id", "UserName", donation.AICUserId);
            return View(donation);
        }

        // GET: Donations/Delete/5
        [Authorize(Roles = "Admin")]
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

            return View(donation);
        }

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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
