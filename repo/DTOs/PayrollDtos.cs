namespace HumanResourcesApi.DTOs;

public class PayrollCreateDto
{
    public string Period { get; set; } = string.Empty;
}

public class PayrollReadDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string EmployeeFullName { get; set; } = string.Empty;

    public string Period { get; set; } = string.Empty;

    public decimal GrossSalary { get; set; }

    public decimal DeductionAmount { get; set; }

    public decimal NetSalary { get; set; }

    public DateTime CreatedAt { get; set; }
}