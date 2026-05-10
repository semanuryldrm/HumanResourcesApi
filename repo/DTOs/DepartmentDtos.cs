namespace HumanResourcesApi.DTOs;

public class DepartmentCreateDto
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}

public class DepartmentUpdateDto
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}

public class DepartmentReadDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public int EmployeeCount { get; set; }
}