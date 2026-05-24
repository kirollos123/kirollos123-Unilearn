using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApp_1.Data;
using UniversityApp_1.Models;

namespace UniversityApp_1.Controllers
{
    [Authorize]
    public class EnrollmentController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public EnrollmentController(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // POST: تسجيل في كورس
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var userId = _userManager.GetUserId(User)!;
            var already = await _db.Enrollments.AnyAsync(e => e.UserId == userId && e.CourseId == courseId);
            if (!already)
            {
                _db.Enrollments.Add(new Enrollment { UserId = userId, CourseId = courseId });
                await _db.SaveChangesAsync();
                TempData["Success"] = "Enrolled successfully!";
            }
            else
            {
                TempData["Info"] = "You are already enrolled in this course.";
            }
            return RedirectToAction("Detail", "Course", new { id = courseId });
        }

        // POST: إلغاء تسجيل
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unenroll(int courseId)
        {
            var userId = _userManager.GetUserId(User)!;
            var enrollment = await _db.Enrollments.FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
            if (enrollment != null)
            {
                _db.Enrollments.Remove(enrollment);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Unenrolled successfully.";
            }
            return RedirectToAction("MyCourses");
        }

        // GET: كورسات الطالب
        public async Task<IActionResult> MyCourses()
        {
            var userId = _userManager.GetUserId(User)!;
            var enrollments = await _db.Enrollments
                .Include(e => e.Course).ThenInclude(c => c!.Department)
                .Include(e => e.Course).ThenInclude(c => c!.Instructors)
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.EnrolledAt)
                .ToListAsync();
            return View(enrollments);
        }

        // GET: Profile
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user!.Id;
            var enrollments = await _db.Enrollments
                .Include(e => e.Course).ThenInclude(c => c!.Department)
                .Where(e => e.UserId == userId)
                .ToListAsync();
            ViewBag.User = user;
            ViewBag.TotalCourses = enrollments.Count;
            ViewBag.Enrollments = enrollments;
            return View();
        }
    }
}
