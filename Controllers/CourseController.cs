using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityApp_1.Data;
using UniversityApp_1.Models;

namespace UniversityApp_1.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly AppDbContext _db;
        public CourseController(AppDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
            => View(await _db.Courses.Include(c => c.Department).ToListAsync());

        public async Task<IActionResult> Detail(int id)
        {
            var course = await _db.Courses
                .Include(c => c.Department)
                .Include(c => c.Instructors)
                .Include(c => c.CrsResults).ThenInclude(r => r.Trainee)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return NotFound();
            return View(course);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name");
            return View();
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _db.Courses.Add(course);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name");
            return View(course);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _db.Courses.FindAsync(id);
            if (course == null) return NotFound();
            ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name", course.DeptId);
            return View(course);
        }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                _db.Courses.Update(course);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name");
            return View(course);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _db.Courses.Include(c => c.Department).FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost, ActionName("Delete"), Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _db.Courses.FindAsync(id);
            if (course != null) _db.Courses.Remove(course);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
