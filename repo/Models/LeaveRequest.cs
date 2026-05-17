using System.ComponentModel.DataAnnotations;

namespace HumanResourcesApi.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public Employee? Employee { get; set; }

        [Required]
        [MaxLength(50)]
        public string LeaveType { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Beklemede";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}