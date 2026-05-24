using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UniversityApp_1.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "decimal(5,2)")]
        public decimal Degree { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal MinDegree { get; set; }
        public int Hours { get; set; }
        public int DeptId { get; set; }
        public Department? Department { get; set; }
        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
        public ICollection<CrsResult> CrsResults { get; set; } = new List<CrsResult>();
    }
}
