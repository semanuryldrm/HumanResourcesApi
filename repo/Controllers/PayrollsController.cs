using HumanResourcesApi.Data;
using HumanResourcesApi.DTOs;
using HumanResourcesApi.Models;
using HumanResourcesApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HumanResourcesApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PayrollsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly PayrollService _payrollService;
    private readonly PayrollPdfService _payrollPdfService;

    public PayrollsController(
        ApplicationDbContext context,
        PayrollService payrollService,
        PayrollPdfService payrollPdfService)
    {
        _context = context;
        _payrollService = payrollService;
        _payrollPdfService = payrollPdfService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PayrollReadDto>>> GetPayrolls()
    {
        var payrolls = await _context.Payrolls
            .Include(p => p.Employee)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PayrollReadDto
            {
                Id = p.Id,
                EmployeeId = p.EmployeeId,
                EmployeeFullName = p.Employee != null
                    ? p.Employee.FirstName + " " + p.Employee.LastName
                    : "",
                Period = p.Period,
                GrossSalary = p.GrossSalary,
                DeductionAmount = p.DeductionAmount,
                NetSalary = p.NetSalary,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        return Ok(payrolls);
    }

    [HttpGet("employee/{employeeId:int}")]
    public async Task<ActionResult<List<PayrollReadDto>>> GetPayrollsByEmployee(int employeeId)
    {
        var employeeExists = await _context.Employees.AnyAsync(e => e.Id == employeeId);

        if (!employeeExists)
        {
            return NotFound("Çalışan bulunamadı.");
        }

        var payrolls = await _context.Payrolls
            .Include(p => p.Employee)
            .Where(p => p.EmployeeId == employeeId)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PayrollReadDto
            {
                Id = p.Id,
                EmployeeId = p.EmployeeId,
                EmployeeFullName = p.Employee != null
                    ? p.Employee.FirstName + " " + p.Employee.LastName
                    : "",
                Period = p.Period,
                GrossSalary = p.GrossSalary,
                DeductionAmount = p.DeductionAmount,
                NetSalary = p.NetSalary,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        return Ok(payrolls);
    }

    [Authorize(Roles = "Admin,IK")]
    [HttpPost("calculate/{employeeId:int}")]
    public async Task<ActionResult<PayrollReadDto>> CalculatePayroll(int employeeId, PayrollCreateDto dto)
    {
        var employee = await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
        {
            return NotFound("Bordro hesaplanacak çalışan bulunamadı.");
        }

        var period = string.IsNullOrWhiteSpace(dto.Period)
            ? DateTime.Now.ToString("yyyy-MM")
            : dto.Period.Trim();

        if (!DateTime.TryParse(period + "-01", out var payrollDate))
        {
            return BadRequest("Bordro dönemi geçersiz. Lütfen 2026-05 formatında bir dönem giriniz.");
        }

        var hireMonth = new DateTime(employee.HireDate.Year, employee.HireDate.Month, 1);
        var payrollMonth = new DateTime(payrollDate.Year, payrollDate.Month, 1);

        if (payrollMonth < hireMonth)
        {
            return BadRequest(
                $"Bu çalışan {employee.HireDate:yyyy-MM-dd} tarihinde işe girdiği için {period} dönemine ait bordro oluşturulamaz."
            );
        }

        var periodExists = await _context.Payrolls
            .AnyAsync(p => p.EmployeeId == employeeId && p.Period == period);

        if (periodExists)
        {
            return BadRequest("Bu çalışan için bu döneme ait bordro zaten oluşturulmuş.");
        }

        var deductionAmount = _payrollService.CalculateDeductionAmount(employee.GrossSalary);
        var netSalary = _payrollService.CalculateNetSalary(employee.GrossSalary);

        var payroll = new Payroll
        {
            EmployeeId = employee.Id,
            Period = period,
            GrossSalary = employee.GrossSalary,
            DeductionAmount = deductionAmount,
            NetSalary = netSalary,
            CreatedAt = DateTime.Now
        };

        _context.Payrolls.Add(payroll);
        await _context.SaveChangesAsync();

        var result = new PayrollReadDto
        {
            Id = payroll.Id,
            EmployeeId = employee.Id,
            EmployeeFullName = employee.FirstName + " " + employee.LastName,
            Period = payroll.Period,
            GrossSalary = payroll.GrossSalary,
            DeductionAmount = payroll.DeductionAmount,
            NetSalary = payroll.NetSalary,
            CreatedAt = payroll.CreatedAt
        };

        return Ok(result);
    }

    [Authorize(Roles = "Admin,IK")]
    [HttpGet("{id:int}/pdf")]
    public async Task<IActionResult> DownloadPayrollPdf(int id)
    {
        var payroll = await _context.Payrolls
            .Include(p => p.Employee)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (payroll == null)
        {
            return NotFound("PDF oluşturulacak bordro kaydı bulunamadı.");
        }

        var pdfBytes = _payrollPdfService.GeneratePayrollPdf(payroll);

        var employeeName = payroll.Employee != null
            ? $"{payroll.Employee.FirstName}-{payroll.Employee.LastName}"
            : "Calisan";

        var fileName = $"Bordro-{employeeName}-{payroll.Period}.pdf";

        return File(pdfBytes, "application/pdf", fileName);
    }

    [Authorize(Roles = "Admin,IK")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePayroll(int id)
    {
        var payroll = await _context.Payrolls.FindAsync(id);

        if (payroll == null)
        {
            return NotFound("Silinecek bordro kaydı bulunamadı.");
        }

        _context.Payrolls.Remove(payroll);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}