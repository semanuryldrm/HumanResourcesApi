namespace HumanResourcesApi.Models;

public class Payroll
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public Employee? Employee { get; set; }

    public string Period { get; set; } = string.Empty;

    public decimal GrossSalary { get; set; }

    public decimal DeductionAmount { get; set; }

    public decimal NetSalary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}