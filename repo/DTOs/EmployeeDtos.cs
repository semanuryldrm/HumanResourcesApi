namespace HumanResourcesApi.DTOs;

public class EmployeeCreateDto
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public DateTime HireDate { get; set; }

    public decimal GrossSalary { get; set; }

    public int DepartmentId { get; set; }
}

public class EmployeeUpdateDto
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public DateTime HireDate { get; set; }

    public decimal GrossSalary { get; set; }

    public int DepartmentId { get; set; }
}

public class EmployeeReadDto
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public DateTime HireDate { get; set; }

    public decimal GrossSalary { get; set; }

    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;
}