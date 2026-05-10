using HumanResourcesApi.Data;
using HumanResourcesApi.DTOs;
using HumanResourcesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HumanResourcesApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase

{
    private readonly ApplicationDbContext _context;

    public DepartmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<DepartmentReadDto>>> GetDepartments()
    {
        var departments = await _context.Departments
            .Include(d => d.Employees)
            .OrderBy(d => d.Name)
            .Select(d => new DepartmentReadDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                CreatedAt = d.CreatedAt,
                EmployeeCount = d.Employees.Count
            })
            .ToListAsync();

        return Ok(departments);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DepartmentReadDto>> GetDepartmentById(int id)
    {
        var department = await _context.Departments
            .Include(d => d.Employees)
            .Where(d => d.Id == id)
            .Select(d => new DepartmentReadDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                CreatedAt = d.CreatedAt,
                EmployeeCount = d.Employees.Count
            })
            .FirstOrDefaultAsync();

        if (department == null)
        {
            return NotFound("Departman bulunamadı.");
        }

        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentReadDto>> CreateDepartment(DepartmentCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Departman adı boş olamaz.");
        }

        var exists = await _context.Departments
            .AnyAsync(d => d.Name.ToLower() == dto.Name.ToLower());

        if (exists)
        {
            return BadRequest("Bu isimde bir departman zaten var.");
        }

        var department = new Department
        {
            Name = dto.Name.Trim(),
            Description = dto.Description,
            CreatedAt = DateTime.Now
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        var result = new DepartmentReadDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            CreatedAt = department.CreatedAt,
            EmployeeCount = 0
        };

        return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDepartment(int id, DepartmentUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Departman adı boş olamaz.");
        }

        var department = await _context.Departments.FindAsync(id);

        if (department == null)
        {
            return NotFound("Güncellenecek departman bulunamadı.");
        }

        department.Name = dto.Name.Trim();
        department.Description = dto.Description;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var department = await _context.Departments
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department == null)
        {
            return NotFound("Silinecek departman bulunamadı.");
        }

        if (department.Employees.Any())
        {
            return BadRequest("Bu departmana bağlı çalışan olduğu için departman silinemez.");
        }

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}