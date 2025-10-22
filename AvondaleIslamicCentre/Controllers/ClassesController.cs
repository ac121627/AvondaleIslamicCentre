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

namespace AvondaleIslamicCentre.Controllers
{
    // Only logged-in users can access this controller
    [Authorize]
    public class ClassesController : Controller
    {
        private readonly AICDbContext _context;
        private const int PageSize = 10; // Controls how many classes show per page

        // Constructor to set up the database context
        public ClassesController(AICDbContext context)
        {
            _context = context;
        }

        // Show a list of all classes with sorting, searching, and pagination
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            // Set up sorting and search options for the view
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ClassSortParm"] = String.IsNullOrEmpty(sortOrder) ? "classname_desc" : "";
            ViewData["StudentsSortParm"] = sortOrder == "students" ? "students_desc" : "students";
            ViewData["CurrentFilter"] = searchString;

            // Get all classes and include the teacher assigned to each
            var classes = _context.Class.Include(c => c.Teacher).AsQueryable();

            // If there’s a search string, filter by class name or teacher’s first name
            if (!String.IsNullOrEmpty(searchString))
            {
                classes = classes.Where(c => c.ClassName.Contains(searchString) ||
                    (c.Teacher != null && c.Teacher.FirstName.Contains(searchString)));
            }

            // Sort results based on the selected sort option
            classes = sortOrder switch
            {
                "classname_desc" => classes.OrderByDescending(c => c.ClassName),
                "students" => classes.OrderBy(c => c.CurrentStudents),
                "students_desc" => classes.OrderByDescending(c => c.CurrentStudents),
                _ => classes.OrderBy(c => c.ClassName),
            };

            // Return paginated results to the view
            return View(await PaginatedList<Class>.CreateAsync(classes.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        // Show details of a specific class
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            // Load class details with its teacher
            var classItem = await _context.Class
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.ClassId == id);

            if (classItem == null)
                return NotFound();

            return View(classItem);
        }

        // Display form for creating a new class (Admins only)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Provide a dropdown list of teachers to assign to the new class
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName");
            return View();
        }

        // Save a new class to the database (Admins only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ClassId,ClassName,Description,CurrentStudents,TeacherId")] Class classItem)
        {
            // If the form data is valid, save it
            if (ModelState.IsValid)
            {
                _context.Add(classItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, reload teacher list and return the form
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", classItem.TeacherId);
            return View(classItem);
        }

        // Display form for editing an existing class (Admins only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            // Find the class to edit
            var classItem = await _context.Class.FindAsync(id);
            if (classItem == null)
                return NotFound();

            // Load list of teachers for the dropdown
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", classItem.TeacherId);
            return View(classItem);
        }

        // Save the edited class details (Admins only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ClassId,ClassName,Description,CurrentStudents,TeacherId")] Class classItem)
        {
            if (id != classItem.ClassId)
                return NotFound();

            // If the input data is valid, update the record
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // If the class no longer exists, return 404
                    if (!ClassExists(classItem.ClassId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // If something goes wrong, re-display the form with teacher list
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", classItem.TeacherId);
            return View(classItem);
        }

        // Display confirmation page before deleting a class (Admins only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            // Load the class and its teacher
            var classItem = await _context.Class
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.ClassId == id);

            if (classItem == null)
                return NotFound();

            return View(classItem);
        }

        // Permanently delete a class after confirmation (Admins only)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the class and remove it if it exists
            var classItem = await _context.Class.FindAsync(id);
            if (classItem != null)
            {
                _context.Class.Remove(classItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Check if a class exists in the database by its ID
        private bool ClassExists(int id)
        {
            return _context.Class.Any(e => e.ClassId == id);
        }
    }
}
