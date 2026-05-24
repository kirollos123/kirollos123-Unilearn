using System.ComponentModel.DataAnnotations;
namespace UniversityApp_1.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        [Display(Name = "Department Name")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Manager { get; set; } = string.Empty;
        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
        public ICollection<Trainee> Trainees { get; set; } = new List<Trainee>();
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
