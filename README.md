# kirollos123-Unilearn
# 🎓 UniLearn — University Management System

<div align="center">

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC%20.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019+-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)

A full-featured **University Management System** built with ASP.NET Core MVC — inspired by Udemy's UI design. Manage Departments, Instructors, Courses, Trainees, and Student Enrollments with role-based access control.

</div>

---

## 📸 Screenshots

| Home Page | Instructors | Departments |
|-----------|-------------|-------------|
| Hero banner + stat cards | Searchable table + avatars | Detail with nested lists |

---

## ✨ Features

- 🔐 **Authentication & Authorization** — ASP.NET Identity with cookie-based sessions
- 👑 **Role-Based Access** — `Admin` (full CRUD) and `Student` (browse & enroll)
- 🏛️ **Departments** — Manage departments with linked instructors, courses, and trainees
- 👨‍🏫 **Instructors** — Full CRUD with photo upload and name search
- 📚 **Courses** — Browse and enroll; track credit hours per department
- 🎓 **Trainees** — Admin-only management with course results (grades)
- 📋 **Enrollments** — Students enroll/unenroll; view grades in personal profile
- 📊 **Admin Dashboard** — High-level stats and quick management access
- 🖼️ **Image Uploads** — Profile photos for instructors and trainees

---

## 🏗️ Architecture

```
┌──────────────────────────────────────┐
│         Presentation Layer           │  Views (.cshtml) + Razor Tag Helpers
├──────────────────────────────────────┤
│          Controller Layer            │  MVC Controllers — Business Logic
├──────────────────────────────────────┤
│         Model / Service Layer        │  Entity Models + AppDbContext (EF Core)
├──────────────────────────────────────┤
│           Data Layer                 │  SQL Server — Code-First Migrations
└──────────────────────────────────────┘
```

---

## 🗄️ Database Schema (ERD Overview)

```
ApplicationUser ──< Enrollment >── Course ──< CrsResult >── Trainee
                                      │
Department ──< Instructor             │
     │                                │
     └────────────────────────────────┘
     (Courses, Instructors, Trainees all belong to a Department)
```

| Entity | Key Fields |
|--------|-----------|
| `Department` | Id, Name, Manager |
| `Instructor` | Id, Name, Salary, Address, Image, DepartmentId, CourseId |
| `Course` | Id, Name, Hours, DepartmentId |
| `Trainee` | Id, Name, Grade, Address, Image, DepartmentId |
| `Enrollment` | Id, UserId, CourseId, Grade, EnrolledAt |
| `CrsResult` | Id, TraineeId, CourseId, Degree |
| `ApplicationUser` | Id, FullName, Email (extends IdentityUser) |

---

## 🔐 Role Permissions Matrix

| Feature | Guest | Student | Admin |
|---------|:-----:|:-------:|:-----:|
| View Instructors / Departments / Courses | ✅ | ✅ | ✅ |
| View Instructor Salary & Address | ❌ | ❌ | ✅ |
| Add / Edit / Delete Instructor | ❌ | ❌ | ✅ |
| Add / Edit / Delete Department | ❌ | ❌ | ✅ |
| Enroll in Courses | ❌ | ✅ | ✅ |
| View My Courses & Profile | ❌ | ✅ | ✅ |
| View / Manage Trainees | ❌ | ❌ | ✅ |
| Admin Dashboard | ❌ | ❌ | ✅ |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server Express / LocalDB
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/kirollos123/kirollos123-Unilearn.git
   cd kirollos123-Unilearn
   ```

2. **Configure the database connection**

   Open `appsettings.json` and update the connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=UniversityDb;Trusted_Connection=True;"
     }
   }
   ```

3. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```
   Then open your browser at `https://localhost:5001`

5. **Seed an Admin user** *(first time only)*

   Register a new account, then manually update the `AspNetUserRoles` table to assign the `Admin` role, or add seeding logic in `Program.cs`.

---

## 📁 Project Structure

```
UniLearn/
├── Controllers/
│   ├── AccountController.cs       # Login, Register, Logout
│   ├── AdminController.cs         # Dashboard
│   ├── DepartmentController.cs    # Department CRUD
│   ├── InstructorController.cs    # Instructor CRUD + search + photo
│   ├── CourseController.cs        # Course CRUD
│   ├── TraineeController.cs       # Trainee CRUD (Admin only)
│   └── EnrollmentController.cs    # Enroll, Unenroll, Profile
├── Models/
│   ├── Department.cs
│   ├── Instructor.cs
│   ├── Course.cs
│   ├── Trainee.cs
│   ├── Enrollment.cs
│   ├── CrsResult.cs
│   ├── ApplicationUser.cs         # Extends IdentityUser
│   └── AppDbContext.cs            # EF Core DbContext
├── Views/
│   ├── Shared/_Layout.cshtml      # Master layout (navbar, footer)
│   ├── Department/
│   ├── Instructor/
│   ├── Course/
│   ├── Trainee/
│   ├── Enrollment/
│   ├── Home/
│   └── Account/
├── wwwroot/
│   └── images/                    # Uploaded instructor & trainee photos
├── Migrations/                    # EF Core auto-generated migrations
├── appsettings.json
└── Program.cs                     # DI registration + middleware pipeline
```

---

## 🛠️ Tech Stack

| Technology | Version | Purpose |
|-----------|---------|---------|
| ASP.NET Core MVC | .NET 8 | Web framework |
| Entity Framework Core | 8.x | ORM — Code-First |
| SQL Server / LocalDB | 2019+ | Database |
| ASP.NET Core Identity | Built-in | Authentication & password hashing |
| Bootstrap | 5.3.0 | Responsive UI |
| Font Awesome | 6.4.0 | Icons |
| Google Fonts (Inter) | — | Typography |
| jQuery Validation | Unobtrusive | Client-side form validation |

---

## 🛡️ Security

- **CSRF Protection** — Anti-forgery tokens on all POST forms
- **Password Hashing** — PBKDF2 + HMAC-SHA256 via ASP.NET Identity
- **SQL Injection Prevention** — EF Core parameterized queries exclusively
- **Role Guards** — `[Authorize(Roles="Admin")]` on sensitive actions
- **Sensitive Data Hiding** — Salary/address hidden from non-Admin views
- **HTTPS** — Enforced via `UseHttpsRedirection()` middleware

---

## 📄 Documentation

A full technical documentation PDF is available in the `/docs` folder, including:
- Architecture diagrams
- ERD (Entity Relationship Diagram)
- Database schema with all columns and constraints
- Role permission matrix
- Authentication & enrollment workflow diagrams
- Controllers & Views reference

---

## 👨‍💻 Author

**Kirollos** — Information Technology Department, 4th Year | 2025/2026

---

## 📝 License

This project is for academic purposes — Information Technology Department, 2025/2026.
