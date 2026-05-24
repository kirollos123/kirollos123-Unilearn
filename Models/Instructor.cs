using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UniversityApp_1.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Salary { get; set; }
        [MaxLength(200)]
        public string? Address { get; set; }
        public int DeptId { get; set; }
        public Department? Department { get; set; }
        public int? CrsId { get; set; }
        public Course? Course { get; set; }
    }
}
