# UniLearn — University Management System

<div align="center">

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC%20.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019+-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)

A full-featured **University Management System** built with ASP.NET Core MVC — inspired by Udemy's UI design. Manage Departments, Instructors, Courses, Trainees, and Student Enrollments with role-based access control.

</div>

---

## Screenshots

| Home | Instructors | Departments |
|------|-------------|-------------|
| ![Home](docs/images/home.png) | ![Instructors](docs/images/instructors.png) | ![Departments](docs/images/departments.png) |

---

## Features

- **Authentication & Authorization** — ASP.NET Identity with cookie-based sessions
- **Role-Based Access** — `Admin` (full CRUD) and `Student` (browse & enroll)
- **Departments** — Manage departments with linked instructors, courses, and trainees
- **Instructors** — Full CRUD with photo upload and name search
- **Courses** — Browse and enroll; track credit hours per department
- **Trainees** — Admin-only management with course results (grades)
- **Enrollments** — Students enroll/unenroll; view grades in personal profile
- **Admin Dashboard** — High-level stats and quick management access
- **Image Uploads** — Profile photos for instructors and trainees

---

## Architecture
