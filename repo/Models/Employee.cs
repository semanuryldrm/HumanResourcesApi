namespace HumanResourcesApi.Models;

public class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public DateTime HireDate { get; set; }

    public decimal GrossSalary { get; set; }

    public int DepartmentId { get; set; }

    public Department? Department { get; set; }

    public List<Payroll> Payrolls { get; set; } = new();
}