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
    [Authorize]
    public class ClassesController : Controller
    {
        private readonly AICDbContext _context;
        private const int PageSize = 10;

        public ClassesController(AICDbContext context)
        {
            _context = context;
        }

        // GET: Classes
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ClassSortParm"] = String.IsNullOrEmpty(sortOrder) ? "classname_desc" : "";
            ViewData["StudentsSortParm"] = sortOrder == "students" ? "students_desc" : "students";
            ViewData["CurrentFilter"] = searchString;

            var classes = _context.Class.Include(c => c.Teacher).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                classes = classes.Where(c => c.ClassName.Contains(searchString) || (c.Teacher != null && c.Teacher.FirstName.Contains(searchString)));
            }

            classes = sortOrder switch
            {
                "classname_desc" => classes.OrderByDescending(c => c.ClassName),
                "students" => classes.OrderBy(c => c.CurrentStudents),
                "students_desc" => classes.OrderByDescending(c => c.CurrentStudents),
                _ => classes.OrderBy(c => c.ClassName),
            };

            return View(await PaginatedList<Class>.CreateAsync(classes.AsNoTracking(), pageNumber ?? 1, PageSize));
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var classItem = await _context.Class
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.ClassId == id);

            if (classItem == null)
                return NotFound();

            return View(classItem);
        }

        // GET: Classes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName");
            return View();
        }

        // POST: Classes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ClassId,ClassName,Description,CurrentStudents,TeacherId")] Class classItem)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(classItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", classItem.TeacherId);
            return View(classItem);
        }

        // GET: Classes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var classItem = await _context.Class.FindAsync(id);
            if (classItem == null)
                return NotFound();

            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", classItem.TeacherId);
            return View(classItem);
        }

        // POST: Classes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ClassId,ClassName,Description,CurrentStudents,TeacherId")] Class classItem)
        {
            if (id != classItem.ClassId)
                return NotFound();

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(classItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(classItem.ClassId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", classItem.TeacherId);
            return View(classItem);
        }

        // GET: Classes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var classItem = await _context.Class
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.ClassId == id);

            if (classItem == null)
                return NotFound();

            return View(classItem);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classItem = await _context.Class.FindAsync(id);
            if (classItem != null)
            {
                _context.Class.Remove(classItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClassExists(int id)
        {
            return _context.Class.Any(e => e.ClassId == id);
        }
    }
}
