using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniversityApp_1.Models;

namespace UniversityApp_1.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var context = services.GetRequiredService<AppDbContext>();

            // ── Roles ──────────────────────────────────────────────
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            // ── Admin User ─────────────────────────────────────────
            var adminEmail = "admin@university.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, FullName = "System Admin" };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // ── Regular Users ──────────────────────────────────────
            var regularUsers = new[]
            {
                ("john.doe@university.com",   "John Doe"),
                ("jane.smith@university.com", "Jane Smith"),
                ("mark.jones@university.com", "Mark Jones"),
                ("sara.ali@university.com",   "Sara Ali"),
                ("omar.hassan@university.com","Omar Hassan"),
            };
            foreach (var (email, fullName) in regularUsers)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new ApplicationUser { UserName = email, Email = email, FullName = fullName };
                    await userManager.CreateAsync(user, "User@123");
                    await userManager.AddToRoleAsync(user, "User");
                }
            }

            // ── Skip if data already exists ────────────────────────
            if (await context.Departments.AnyAsync()) return;

            // ── Departments ────────────────────────────────────────
            var departments = new List<Department>
            {
                new() { Name = "Computer Science",       Manager = "Dr. Ahmed Kamal" },
                new() { Name = "Information Technology", Manager = "Dr. Mona Samir" },
                new() { Name = "Software Engineering",   Manager = "Dr. Khaled Nour" },
                new() { Name = "Cybersecurity",          Manager = "Dr. Rania Fawzy" },
                new() { Name = "Data Science",           Manager = "Dr. Tarek Mansour" },
            };
            await context.Departments.AddRangeAsync(departments);
            await context.SaveChangesAsync();

            var cs  = departments[0];
            var it  = departments[1];
            var se  = departments[2];
            var cyb = departments[3];
            var ds  = departments[4];

            // ── Courses ────────────────────────────────────────────
            var courses = new List<Course>
            {
                // CS
                new() { Name = "Introduction to Programming", Degree = 100, MinDegree = 50, Hours = 3, DeptId = cs.Id },
                new() { Name = "Data Structures",             Degree = 100, MinDegree = 50, Hours = 3, DeptId = cs.Id },
                new() { Name = "Algorithms",                  Degree = 100, MinDegree = 50, Hours = 3, DeptId = cs.Id },
                new() { Name = "Operating Systems",           Degree = 100, MinDegree = 50, Hours = 3, DeptId = cs.Id },
                // IT
                new() { Name = "Networking Fundamentals",     Degree = 100, MinDegree = 50, Hours = 3, DeptId = it.Id },
                new() { Name = "Database Management",         Degree = 100, MinDegree = 50, Hours = 3, DeptId = it.Id },
                new() { Name = "Web Development",             Degree = 100, MinDegree = 50, Hours = 3, DeptId = it.Id },
                // SE
                new() { Name = "Software Design Patterns",    Degree = 100, MinDegree = 50, Hours = 3, DeptId = se.Id },
                new() { Name = "Agile Development",           Degree = 100, MinDegree = 50, Hours = 3, DeptId = se.Id },
                new() { Name = "Software Testing",            Degree = 100, MinDegree = 50, Hours = 3, DeptId = se.Id },
                // Cybersecurity
                new() { Name = "Network Security",            Degree = 100, MinDegree = 50, Hours = 3, DeptId = cyb.Id },
                new() { Name = "Ethical Hacking",             Degree = 100, MinDegree = 50, Hours = 3, DeptId = cyb.Id },
                new() { Name = "Cryptography",                Degree = 100, MinDegree = 50, Hours = 3, DeptId = cyb.Id },
                // Data Science
                new() { Name = "Machine Learning",            Degree = 100, MinDegree = 50, Hours = 3, DeptId = ds.Id },
                new() { Name = "Data Analysis",               Degree = 100, MinDegree = 50, Hours = 3, DeptId = ds.Id },
                new() { Name = "Deep Learning",               Degree = 100, MinDegree = 50, Hours = 3, DeptId = ds.Id },
            };
            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();

            // ── Instructors ────────────────────────────────────────
            var instructors = new List<Instructor>
            {
                new() { Name = "Dr. Ahmed Kamal",   Salary = 8000,  Address = "Cairo",       DeptId = cs.Id,  CrsId = courses[0].Id },
                new() { Name = "Dr. Mona Samir",    Salary = 7500,  Address = "Giza",        DeptId = it.Id,  CrsId = courses[4].Id },
                new() { Name = "Dr. Khaled Nour",   Salary = 8500,  Address = "Alexandria",  DeptId = se.Id,  CrsId = courses[7].Id },
                new() { Name = "Dr. Rania Fawzy",   Salary = 9000,  Address = "Cairo",       DeptId = cyb.Id, CrsId = courses[10].Id },
                new() { Name = "Dr. Tarek Mansour", Salary = 8200,  Address = "Mansoura",    DeptId = ds.Id,  CrsId = courses[13].Id },
                new() { Name = "Eng. Sara Hossam",  Salary = 6000,  Address = "Cairo",       DeptId = cs.Id,  CrsId = courses[1].Id },
                new() { Name = "Eng. Omar Fathy",   Salary = 6500,  Address = "Giza",        DeptId = it.Id,  CrsId = courses[5].Id },
                new() { Name = "Eng. Nadia Youssef", Salary = 7000, Address = "Alexandria",  DeptId = se.Id,  CrsId = courses[8].Id },
                new() { Name = "Eng. Hassan Adel",  Salary = 7200,  Address = "Tanta",       DeptId = cyb.Id, CrsId = courses[11].Id },
                new() { Name = "Eng. Laila Mostafa", Salary = 6800, Address = "Cairo",       DeptId = ds.Id,  CrsId = courses[14].Id },
                new() { Name = "Dr. Youssef Gamal", Salary = 8100,  Address = "Aswan",       DeptId = cs.Id,  CrsId = courses[2].Id },
                new() { Name = "Dr. Heba Ramadan",  Salary = 7900,  Address = "Luxor",       DeptId = it.Id,  CrsId = courses[6].Id },
                new() { Name = "Dr. Amr Sayed",     Salary = 8300,  Address = "Cairo",       DeptId = se.Id,  CrsId = courses[9].Id },
                new() { Name = "Dr. Dina Waheed",   Salary = 8700,  Address = "Giza",        DeptId = cyb.Id, CrsId = courses[12].Id },
                new() { Name = "Dr. Fady Nabil",    Salary = 8400,  Address = "Port Said",   DeptId = ds.Id,  CrsId = courses[15].Id },
            };
            await context.Instructors.AddRangeAsync(instructors);
            await context.SaveChangesAsync();

            // ── Trainees ───────────────────────────────────────────
            var trainees = new List<Trainee>
            {
                new() { Name = "Ali Mohamed",      Address = "Cairo",      Grade = "A",  DeptId = cs.Id },
                new() { Name = "Nour Ahmed",       Address = "Giza",       Grade = "B+", DeptId = cs.Id },
                new() { Name = "Kareem Hassan",    Address = "Alex",       Grade = "A-", DeptId = cs.Id },
                new() { Name = "Yasmin Khaled",    Address = "Tanta",      Grade = "B",  DeptId = cs.Id },
                new() { Name = "Mohamed Tarek",    Address = "Cairo",      Grade = "A+", DeptId = cs.Id },
                new() { Name = "Rana Samir",       Address = "Cairo",      Grade = "B+", DeptId = it.Id },
                new() { Name = "Ahmed Nabil",      Address = "Mansoura",   Grade = "A",  DeptId = it.Id },
                new() { Name = "Menna Fathy",      Address = "Giza",       Grade = "C+", DeptId = it.Id },
                new() { Name = "Hossam Adel",      Address = "Alex",       Grade = "B",  DeptId = it.Id },
                new() { Name = "Salma Waheed",     Address = "Cairo",      Grade = "A-", DeptId = it.Id },
                new() { Name = "Bishoy Ramzy",     Address = "Cairo",      Grade = "B",  DeptId = se.Id },
                new() { Name = "Mariam Youssef",   Address = "Aswan",      Grade = "A",  DeptId = se.Id },
                new() { Name = "Ziad Mostafa",     Address = "Luxor",      Grade = "B+", DeptId = se.Id },
                new() { Name = "Dalia Sayed",      Address = "Cairo",      Grade = "A-", DeptId = se.Id },
                new() { Name = "Karim Gamal",      Address = "Giza",       Grade = "C",  DeptId = se.Id },
                new() { Name = "Farida Hossam",    Address = "Port Said",  Grade = "A",  DeptId = cyb.Id },
                new() { Name = "Mahmoud Fawzy",    Address = "Cairo",      Grade = "B",  DeptId = cyb.Id },
                new() { Name = "Nadia Omar",       Address = "Alex",       Grade = "A+", DeptId = cyb.Id },
                new() { Name = "Sherif Kamal",     Address = "Tanta",      Grade = "B-", DeptId = cyb.Id },
                new() { Name = "Amira Hassan",     Address = "Cairo",      Grade = "A",  DeptId = cyb.Id },
                new() { Name = "Tarek Adel",       Address = "Giza",       Grade = "B+", DeptId = ds.Id },
                new() { Name = "Noha Nabil",       Address = "Cairo",      Grade = "A-", DeptId = ds.Id },
                new() { Name = "Omar Ramadan",     Address = "Alex",       Grade = "B",  DeptId = ds.Id },
                new() { Name = "Hana Samir",       Address = "Mansoura",   Grade = "A",  DeptId = ds.Id },
                new() { Name = "Youssef Waheed",   Address = "Cairo",      Grade = "B+", DeptId = ds.Id },
            };
            await context.Trainees.AddRangeAsync(trainees);
            await context.SaveChangesAsync();

            // ── Course Results ─────────────────────────────────────
            var random = new Random(42);
            var results = new List<CrsResult>();
            foreach (var trainee in trainees)
            {
                // كل trainee عنده 3 course results في الـ department بتاعته
                var deptCourses = courses.Where(c => c.DeptId == trainee.DeptId).Take(3).ToList();
                foreach (var course in deptCourses)
                {
                    results.Add(new CrsResult
                    {
                        TraineeId = trainee.Id,
                        CrsId = course.Id,
                        Degree = random.Next(50, 100)
                    });
                }
            }
            await context.CrsResults.AddRangeAsync(results);
            await context.SaveChangesAsync();
        }
    }
}
