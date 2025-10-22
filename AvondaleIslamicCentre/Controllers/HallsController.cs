using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AvondaleIslamicCentre.Areas.Identity.Data;
using AvondaleIslamicCentre.Models;
using Microsoft.AspNetCore.Authorization;

namespace AvondaleIslamicCentre.Controllers
{
    // Only logged-in users can access this controller
    [Authorize]
    public class HallsController : Controller
    {
        private readonly AICDbContext _context;
        private const int PageSize = 10; // Controls how many halls show per page

        // Constructor sets up the database context
        public HallsController(AICDbContext context)
        {
            _context = context;
        }

        // Show a list of all halls with search, sorting, and pagination
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            // Save sorting and search info for the view
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CapacitySortParm"] = sortOrder == "capacity" ? "capacity_desc" : "capacity";
            ViewData["CurrentFilter"] = searchString;

            // Start with all halls
            var halls = from h in _context.Hall select h;

            // Filter by name if a search term is provided
            if (!String.IsNullOrEmpty(searchString))
            {
                halls = halls.Where(h => h.Name.Contains(searchString));
            }

            // Sort based on selected order
            halls = sortOrder switch
            {
                "name_desc" => halls.OrderByDescending(h => h.Name),
                "capacity" => halls.OrderBy(h => h.Capacity),
                "capacity_desc" => halls.OrderByDescending(h => h.Capacity),
                _ => halls.OrderBy(h => h.Name),
            };

            // Return paginated results to the view
            return View(await PaginatedList<Hall>.CreateAsync(halls.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        // Show details for a specific hall
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the hall by its ID
            var hall = await _context.Hall
                .FirstOrDefaultAsync(m => m.HallId == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        // Display form for creating a new hall (Admins only)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Save a new hall to the database (Admins only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("HallId,Name,Capacity")] Hall hall)
        {
            // If the form data is valid, save the new hall
            if (ModelState.IsValid)
            {
                _context.Add(hall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hall);
        }

        // Display form for editing an existing hall (Admins only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the hall to edit
            var hall = await _context.Hall.FindAsync(id);
            if (hall == null)
            {
                return NotFound();
            }
            return View(hall);
        }

        // Save the edited hall details (Admins only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("HallId,Name,Capacity")] Hall hall)
        {
            if (id != hall.HallId)
            {
                return NotFound();
            }

            // If form data is valid, update the record
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle case where the hall no longer exists
                    if (!HallExists(hall.HallId))
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
            return View(hall);
        }

        // Display confirmation page before deleting a hall (Admins only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the hall to delete
            var hall = await _context.Hall
                .FirstOrDefaultAsync(m => m.HallId == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        // Permanently delete a hall after confirmation (Admins only)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the hall and remove it if found
            var hall = await _context.Hall.FindAsync(id);
            if (hall != null)
            {
                _context.Hall.Remove(hall);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Check if a hall exists in the database
        private bool HallExists(int id)
        {
            return _context.Hall.Any(e => e.HallId == id);
        }
    }
}
