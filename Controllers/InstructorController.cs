using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityApp_1.Data;
using UniversityApp_1.Models;

namespace UniversityApp_1.Controllers
{
    [Authorize]
    public class InstructorController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public InstructorController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // Assignment 3: Search by name
        public async Task<IActionResult> Index(string? searchName)
        {
            ViewBag.SearchName = searchName;
            var query = _db.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
                query = query.Where(i => i.Name.Contains(searchName));

            return View(await query.ToListAsync());
        }

        // Assignment 2: Detail by ID (Strong Type View)
        public async Task<IActionResult> Detail(int id)
        {
            var instructor = await _db.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (instructor == null) return NotFound();
            return View(instructor);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name");
            ViewBag.Courses = new SelectList(_db.Courses, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instructor instructor, IFormFile? imageFile)
        {
            if (imageFile != null)
                instructor.Image = await SaveImage(imageFile);

            if (ModelState.IsValid)
            {
                _db.Instructors.Add(instructor);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name");
            ViewBag.Courses = new SelectList(_db.Courses, "Id", "Name");
            return View(instructor);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var instructor = await _db.Instructors.FindAsync(id);
            if (instructor == null) return NotFound();
            ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name", instructor.DeptId);
            ViewBag.Courses = new SelectList(_db.Courses, "Id", "Name", instructor.CrsId);
            return View(instructor);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Instructor instructor, IFormFile? imageFile)
        {
            if (imageFile != null)
                instructor.Image = await SaveImage(imageFile);

            if (ModelState.IsValid)
            {
                _db.Instructors.Update(instructor);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name");
            ViewBag.Courses = new SelectList(_db.Courses, "Id", "Name");
            return View(instructor);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var instructor = await _db.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (instructor == null) return NotFound();
            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _db.Instructors.FindAsync(id);
            if (instructor != null) _db.Instructors.Remove(instructor);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "instructors");
            Directory.CreateDirectory(uploadsDir);
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsDir, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return "/uploads/instructors/" + fileName;
        }
    }
}
