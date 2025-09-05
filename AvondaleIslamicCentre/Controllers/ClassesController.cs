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

        public ClassesController(AICDbContext context)
        {
            _context = context;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            var classesWithTeachers = _context.Class.Include(c => c.Teacher);
            return View(await classesWithTeachers.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName");
            return View();
        }

        // POST: Classes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassId,ClassName,Description,CurrentStudents,TeacherId")] Class classItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", classItem.TeacherId);
            return View(classItem);
        }

        // GET: Classes/Edit/5
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
        public async Task<IActionResult> Edit(int id, [Bind("ClassId,ClassName,Description,CurrentStudents,TeacherId")] Class classItem)
        {
            if (id != classItem.ClassId)
                return NotFound();

            if (ModelState.IsValid)
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
