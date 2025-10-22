using AvondaleIslamicCentre.Areas.Identity.Data;
using AvondaleIslamicCentre.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AvondaleIslamicCentre.Controllers
{
    // Only logged-in users can access this controller
    [Authorize]
    public class TeachersController : Controller
    {
        private readonly AICDbContext _context;
        private const int PageSize = 10; // Number of teachers shown per page

        // Constructor to set up database context
        public TeachersController(AICDbContext context)
        {
            _context = context;
        }

        // Shows a paginated and searchable list of teachers
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            // Save sorting and searching info for the view
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FirstNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
            ViewData["LastNameSortParm"] = sortOrder == "lastname" ? "lastname_desc" : "lastname";
            ViewData["CurrentFilter"] = searchString;

            // Get all teachers from the database
            var teachers = _context.Teachers.AsQueryable();

            // Filter by first or last name if a search is entered
            if (!String.IsNullOrEmpty(searchString))
            {
                teachers = teachers.Where(t =>
                    t.FirstName.Contains(searchString) || t.LastName.Contains(searchString));
            }

            // Sort by first or last name depending on user choice
            teachers = sortOrder switch
            {
                "firstname_desc" => teachers.OrderByDescending(t => t.FirstName),
                "lastname" => teachers.OrderBy(t => t.LastName),
                "lastname_desc" => teachers.OrderByDescending(t => t.LastName),
                _ => teachers.OrderBy(t => t.FirstName),
            };

            // Return the paginated list to the view
            return View(await PaginatedList<Teacher>.CreateAsync(teachers.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        // Show details for a single teacher
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the teacher by ID
            var teacher = await _context.Teachers.FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // Show form to create a new teacher (Admins only)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Handle POST request to add a new teacher (Admins only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("TeacherId,FirstName,LastName,Email,PhoneNumber")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                // Add the teacher to the database
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // If validation fails, show the same form again
            return View(teacher);
        }

        // Show form to edit an existing teacher (Admins only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the teacher by ID
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // Handle POST request to update a teacher (Admins only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherId,FirstName,LastName,Email,PhoneNumber")] Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update teacher info in the database
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle case where teacher no longer exists
                    if (!TeacherExists(teacher.TeacherId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Redirect back to the teacher list
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, show the same form again
            return View(teacher);
        }

        // Show confirmation page before deleting a teacher (Admins only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the teacher by ID
            var teacher = await _context.Teachers.FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // Permanently delete a teacher from the database (Admins only)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if a teacher exists
        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.TeacherId == id);
        }
    }
}

