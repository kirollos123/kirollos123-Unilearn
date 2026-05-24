using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UniversityApp_1.Models
{
    public class CrsResult
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Degree { get; set; }
        public int CrsId { get; set; }
        public Course? Course { get; set; }
        public int TraineeId { get; set; }
        public Trainee? Trainee { get; set; }
    }
}
