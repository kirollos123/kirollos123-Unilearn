using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApp_1.Data;
using UniversityApp_1.Models;

namespace UniversityApp_1.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Department
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.Instructors)
                .Include(d => d.Trainees)
                .Include(d => d.Courses)
                .ToListAsync();
            return View(departments);
        }

        // GET: /Department/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var dept = await _context.Departments
                .Include(d => d.Instructors)
                .Include(d => d.Trainees)
                .Include(d => d.Courses)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (dept == null) return NotFound();
            return View(dept);
        }

        // GET: /Department/Create
        public IActionResult Create() => View();

        // POST: /Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: /Department/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();
            return View(dept);
        }

        // POST: /Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Departments.Update(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: /Department/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var dept = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id);
            if (dept == null) return NotFound();
            return View(dept);
        }

        // POST: /Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept != null)
            {
                _context.Departments.Remove(dept);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
