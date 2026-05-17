using HumanResourcesApi.Data;
using HumanResourcesApi.DTOs;
using HumanResourcesApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HumanResourcesApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmployeesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeReadDto>>> GetEmployees()
    {
        var employees = await _context.Employees
            .Include(e => e.Department)
            .OrderBy(e => e.FirstName)
            .Select(e => new EmployeeReadDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                FullName = e.FirstName + " " + e.LastName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                HireDate = e.HireDate,
                GrossSalary = e.GrossSalary,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department != null ? e.Department.Name : ""
            })
            .ToListAsync();

        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeReadDto>> GetEmployeeById(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.Department)
            .Where(e => e.Id == id)
            .Select(e => new EmployeeReadDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                FullName = e.FirstName + " " + e.LastName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                HireDate = e.HireDate,
                GrossSalary = e.GrossSalary,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department != null ? e.Department.Name : ""
            })
            .FirstOrDefaultAsync();

        if (employee == null)
        {
            return NotFound("Çalışan bulunamadı.");
        }

        return Ok(employee);
    }

    [Authorize(Roles = "Admin,IK")]
    [HttpPost]
    public async Task<ActionResult<EmployeeReadDto>> CreateEmployee(EmployeeCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName))
        {
            return BadRequest("Çalışan adı boş olamaz.");
        }

        if (string.IsNullOrWhiteSpace(dto.LastName))
        {
            return BadRequest("Çalışan soyadı boş olamaz.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest("E-posta adresi boş olamaz.");
        }

        if (dto.GrossSalary <= 0)
        {
            return BadRequest("Brüt maaş 0'dan büyük olmalıdır.");
        }

        var departmentExists = await _context.Departments
            .AnyAsync(d => d.Id == dto.DepartmentId);

        if (!departmentExists)
        {
            return BadRequest("Girilen DepartmentId değerine ait departman bulunamadı.");
        }

        var emailExists = await _context.Employees
            .AnyAsync(e => e.Email.ToLower() == dto.Email.ToLower());

        if (emailExists)
        {
            return BadRequest("Bu e-posta adresiyle kayıtlı bir çalışan zaten var.");
        }

        var employee = new Employee
        {
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            Email = dto.Email.Trim(),
            PhoneNumber = dto.PhoneNumber,
            HireDate = dto.HireDate,
            GrossSalary = dto.GrossSalary,
            DepartmentId = dto.DepartmentId
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        var result = await _context.Employees
            .Include(e => e.Department)
            .Where(e => e.Id == employee.Id)
            .Select(e => new EmployeeReadDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                FullName = e.FirstName + " " + e.LastName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                HireDate = e.HireDate,
                GrossSalary = e.GrossSalary,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department != null ? e.Department.Name : ""
            })
            .FirstAsync();

        return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, result);
    }

    [Authorize(Roles = "Admin,IK")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEmployee(int id, EmployeeUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName))
        {
            return BadRequest("Çalışan adı boş olamaz.");
        }

        if (string.IsNullOrWhiteSpace(dto.LastName))
        {
            return BadRequest("Çalışan soyadı boş olamaz.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest("E-posta adresi boş olamaz.");
        }

        if (dto.GrossSalary <= 0)
        {
            return BadRequest("Brüt maaş 0'dan büyük olmalıdır.");
        }

        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound("Güncellenecek çalışan bulunamadı.");
        }

        var departmentExists = await _context.Departments
            .AnyAsync(d => d.Id == dto.DepartmentId);

        if (!departmentExists)
        {
            return BadRequest("Girilen DepartmentId değerine ait departman bulunamadı.");
        }

        var emailExists = await _context.Employees
            .AnyAsync(e => e.Id != id && e.Email.ToLower() == dto.Email.ToLower());

        if (emailExists)
        {
            return BadRequest("Bu e-posta adresi başka bir çalışanda kullanılıyor.");
        }

        employee.FirstName = dto.FirstName.Trim();
        employee.LastName = dto.LastName.Trim();
        employee.Email = dto.Email.Trim();
        employee.PhoneNumber = dto.PhoneNumber;
        employee.HireDate = dto.HireDate;
        employee.GrossSalary = dto.GrossSalary;
        employee.DepartmentId = dto.DepartmentId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin,IK")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound("Silinecek çalışan bulunamadı.");
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}