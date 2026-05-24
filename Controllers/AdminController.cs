using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApp_1.Data;
using UniversityApp_1.Models;

namespace UniversityApp_1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // ── Dashboard ──────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var vm = new AdminDashboardViewModel
            {
                TotalUsers       = await _userManager.Users.CountAsync(),
                TotalDepartments = await _context.Departments.CountAsync(),
                TotalCourses     = await _context.Courses.CountAsync(),
                TotalInstructors = await _context.Instructors.CountAsync(),
                TotalTrainees    = await _context.Trainees.CountAsync(),
                TotalRoles       = await _roleManager.Roles.CountAsync(),
                RecentUsers      = await _userManager.Users.OrderByDescending(u => u.Email).Take(5).ToListAsync()
            };
            return View(vm);
        }

        // ── Users List ─────────────────────────────────────────────
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userVms = new List<UserViewModel>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                userVms.Add(new UserViewModel
                {
                    Id       = u.Id,
                    FullName = u.FullName,
                    Email    = u.Email ?? "",
                    Roles    = roles.ToList()
                });
            }
            return View(userVms);
        }

        // ── Delete User ────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null) await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Users));
        }

        // ── Change Role ────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (await _roleManager.RoleExistsAsync(newRole))
                await _userManager.AddToRoleAsync(user, newRole);
            return RedirectToAction(nameof(Users));
        }

        // ── Roles ──────────────────────────────────────────────────
        public async Task<IActionResult> Roles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName) && !await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            return RedirectToAction(nameof(Roles));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null) await _roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Roles));
        }
    }

    // ── ViewModels ─────────────────────────────────────────────────
    public class AdminDashboardViewModel
    {
        public int TotalUsers       { get; set; }
        public int TotalDepartments { get; set; }
        public int TotalCourses     { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalTrainees    { get; set; }
        public int TotalRoles       { get; set; }
        public List<ApplicationUser> RecentUsers { get; set; } = new();
    }

    public class UserViewModel
    {
        public string Id       { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email    { get; set; } = "";
        public List<string> Roles { get; set; } = new();
    }
}
