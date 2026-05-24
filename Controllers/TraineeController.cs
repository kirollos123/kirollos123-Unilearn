using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityApp_1.Data;
using UniversityApp_1.Models;

namespace UniversityApp_1.Controllers
{
    [Authorize]
    public class TraineeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public TraineeController(AppDbContext db, IWebHostEnvironment env) { _db = db; _env = env; }

        public async Task<IActionResult> Index()
            => View(await _db.Trainees.Include(t => t.Department).ToListAsync());

        public async Task<IActionResult> Detail(int id)
        {
            var t = await _db.Trainees.Include(t => t.Department).Include(t => t.CrsResults).ThenInclude(r => r.Course).FirstOrDefaultAsync(t => t.Id == id);
            if (t == null) return NotFound();
            return View(t);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() { ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name"); return View(); }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trainee trainee, IFormFile? imageFile)
        {
            if (imageFile != null) trainee.Image = await SaveImage(imageFile);
            if (ModelState.IsValid) { _db.Trainees.Add(trainee); await _db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
            ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name");
            return View(trainee);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id) { var t = await _db.Trainees.FindAsync(id); if (t == null) return NotFound(); ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name", t.DeptId); return View(t); }

        [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Trainee trainee, IFormFile? imageFile)
        {
            if (imageFile != null) trainee.Image = await SaveImage(imageFile);
            if (ModelState.IsValid) { _db.Trainees.Update(trainee); await _db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
            ViewBag.Departments = new SelectList(_db.Departments, "Id", "Name");
            return View(trainee);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) { var t = await _db.Trainees.Include(t => t.Department).FirstOrDefaultAsync(t => t.Id == id); if (t == null) return NotFound(); return View(t); }

        [HttpPost, ActionName("Delete"), Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) { var t = await _db.Trainees.FindAsync(id); if (t != null) _db.Trainees.Remove(t); await _db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }

        private async Task<string> SaveImage(IFormFile file)
        {
            var dir = Path.Combine(_env.WebRootPath, "uploads", "trainees");
            Directory.CreateDirectory(dir);
            var name = Guid.NewGuid() + Path.GetExtension(file.FileName);
            using var s = new FileStream(Path.Combine(dir, name), FileMode.Create);
            await file.CopyToAsync(s);
            return "/uploads/trainees/" + name;
        }
    }
}
