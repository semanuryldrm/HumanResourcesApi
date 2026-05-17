using System.ComponentModel.DataAnnotations;

namespace HumanResourcesApi.DTOs
{
    public class LeaveRequestCreateDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string LeaveType { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }

    public class LeaveRequestUpdateStatusDto
    {
        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = string.Empty;
    }

    public class LeaveRequestListDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeFullName { get; set; } = string.Empty;

        public string LeaveType { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TotalDays { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}