using System.ComponentModel.DataAnnotations;
namespace UniversityApp_1.Models
{
    public class Trainee
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        [MaxLength(200)]
        public string? Address { get; set; }
        [MaxLength(50)]
        public string? Grade { get; set; }
        public int DeptId { get; set; }
        public Department? Department { get; set; }
        public ICollection<CrsResult> CrsResults { get; set; } = new List<CrsResult>();
    }
}
