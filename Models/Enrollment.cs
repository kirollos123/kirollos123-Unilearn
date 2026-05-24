using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UniversityApp_1.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Grade { get; set; }
    }
}
