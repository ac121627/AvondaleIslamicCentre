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
    public class StudentsController : Controller
    {
        private readonly AICDbContext _context;
        private const int PageSize = 10; // Controls how many students are shown per page

        // Constructor sets up the database context
        public StudentsController(AICDbContext context)
        {
            _context = context;
        }

        // Show a list of all students with sorting, searching, and pagination
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            // Save sorting and search info for the view
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FirstNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
            ViewData["LastNameSortParm"] = sortOrder == "lastname" ? "lastname_desc" : "lastname";
            ViewData["CurrentFilter"] = searchString;

            // Get all students including their class and teacher
            var students = _context.Students
                .Include(s => s.Class)
                .Include(s => s.Teacher)
                .AsQueryable();

            // Filter by first or last name if a search term is entered
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s =>
                    (s.FirstName != null && s.FirstName.Contains(searchString)) ||
                    (s.LastName != null && s.LastName.Contains(searchString)));
            }

            // Sort the results by the chosen field
            students = sortOrder switch
            {
                "firstname_desc" => students.OrderByDescending(s => s.FirstName),
                "lastname" => students.OrderBy(s => s.LastName),
                "lastname_desc" => students.OrderByDescending(s => s.LastName),
                _ => students.OrderBy(s => s.FirstName),
            };

            // Return the paginated results to the view
            return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        // Show details for a specific student
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load student with their class and teacher info
            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // Display form to create a new student (Admins only)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Provide dropdown lists for classes and teachers
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "ClassName");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName");
            return View();
        }

        // Save a new student to the database (Admins only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("StudentId,GuardianFirstName,GuardianLastName,FirstName,LastName,Email,PhoneNumber,Gender,Ethnicity,QuranNazira,QuranHifz,DateOfBirth,Address,ClassId,TeacherId")] Student student)
        {
            if (ModelState.IsValid)
            {
                // Check if the chosen class and teacher exist
                if (!_context.Class.Any(c => c.ClassId == student.ClassId) || !_context.Teachers.Any(t => t.TeacherId == student.TeacherId))
                {
                    ModelState.AddModelError(string.Empty, "Selected Class or Teacher does not exist.");
                    ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "ClassName", student.ClassId);
                    ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", student.TeacherId);
                    return View(student);
                }

                // Add the student and save to the database
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, reload dropdowns and show the form again
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "ClassName", student.ClassId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", student.TeacherId);
            return View(student);
        }

        // Display form to edit an existing student (Admins only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the student by ID
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            // Provide dropdown lists with current selections
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "ClassName", student.ClassId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", student.TeacherId);
            return View(student);
        }

        // Save the edited student details (Admins only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,GuardianFirstName,GuardianLastName,FirstName,LastName,Email,PhoneNumber,Gender,Ethnicity,QuranNazira,QuranHifz,DateOfBirth,Address,ClassId,TeacherId")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the student in the database
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle case where the student no longer exists
                    if (!StudentExists(student.StudentId))
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

            // Reload dropdowns if validation fails
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "ClassName", student.ClassId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", student.TeacherId);
            return View(student);
        }

        // Display confirmation page before deleting a student (Admins only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load student info along with their class and teacher
            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // Permanently delete a student after confirmation (Admins only)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find and remove the student if found
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Check if a student exists in the database by their ID
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
