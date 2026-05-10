namespace HumanResourcesApi.Models;

public class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<Employee> Employees { get; set; } = new();
}