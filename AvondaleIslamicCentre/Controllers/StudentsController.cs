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
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly AICDbContext _context;
        private const int PageSize = 10;

        public StudentsController(AICDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FirstNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
            ViewData["LastNameSortParm"] = sortOrder == "lastname" ? "lastname_desc" : "lastname";
            ViewData["CurrentFilter"] = searchString;

            var students = _context.Students.Include(s => s.Class).Include(s => s.Teacher).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => (s.FirstName != null && s.FirstName.Contains(searchString)) || (s.LastName != null && s.LastName.Contains(searchString)));
            }

            students = sortOrder switch
            {
                "firstname_desc" => students.OrderByDescending(s => s.FirstName),
                "lastname" => students.OrderBy(s => s.LastName),
                "lastname_desc" => students.OrderByDescending(s => s.LastName),
                _ => students.OrderBy(s => s.FirstName),
            };

            return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        // GET: Students/Details/5 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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

        // GET: Students/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "ClassName");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName");
            return View();
        }

        // POST: Students/Create
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("StudentId,GuardianFirstName,GuardianLastName,FirstName,LastName,Email,PhoneNumber,Gender,Ethnicity,QuranNazira,QuranHifz,DateOfBirth,Address,ClassId,TeacherId")] Student student)
        {
            if (ModelState.IsValid)
            {
                // ensure class and teacher exist
                if (!_context.Class.Any(c => c.ClassId == student.ClassId) || !_context.Teachers.Any(t => t.TeacherId == student.TeacherId))
                {
                    ModelState.AddModelError(string.Empty, "Selected Class or Teacher does not exist.");
                    ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "ClassName", student.ClassId);
                    ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", student.TeacherId);
                    return View(student);
                }

                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "ClassName", student.ClassId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", student.TeacherId);
            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "ClassName", student.ClassId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", student.TeacherId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
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
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "ClassName", student.ClassId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", student.TeacherId);
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
