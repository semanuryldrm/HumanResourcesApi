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
public class LeaveRequestsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LeaveRequestsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaveRequestListDto>>> GetLeaveRequests()
    {
        var leaveRequests = await _context.LeaveRequests
            .Include(l => l.Employee)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new LeaveRequestListDto
            {
                Id = l.Id,
                EmployeeId = l.EmployeeId,
                EmployeeFullName = l.Employee != null
                    ? l.Employee.FirstName + " " + l.Employee.LastName
                    : "-",
                LeaveType = l.LeaveType,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                TotalDays = EF.Functions.DateDiffDay(l.StartDate, l.EndDate) + 1,
                Description = l.Description,
                Status = l.Status,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync();

        return Ok(leaveRequests);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveRequestListDto>> GetLeaveRequest(int id)
    {
        var leaveRequest = await _context.LeaveRequests
            .Include(l => l.Employee)
            .Where(l => l.Id == id)
            .Select(l => new LeaveRequestListDto
            {
                Id = l.Id,
                EmployeeId = l.EmployeeId,
                EmployeeFullName = l.Employee != null
                    ? l.Employee.FirstName + " " + l.Employee.LastName
                    : "-",
                LeaveType = l.LeaveType,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                TotalDays = EF.Functions.DateDiffDay(l.StartDate, l.EndDate) + 1,
                Description = l.Description,
                Status = l.Status,
                CreatedAt = l.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (leaveRequest == null)
        {
            return NotFound("İzin kaydı bulunamadı.");
        }

        return Ok(leaveRequest);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLeaveRequest(LeaveRequestCreateDto dto)
    {
        if (dto.EndDate < dto.StartDate)
        {
            return BadRequest("Bitiş tarihi başlangıç tarihinden önce olamaz.");
        }

        var employeeExists = await _context.Employees
            .AnyAsync(e => e.Id == dto.EmployeeId);

        if (!employeeExists)
        {
            return BadRequest("Seçilen çalışan bulunamadı.");
        }

        var leaveRequest = new LeaveRequest
        {
            EmployeeId = dto.EmployeeId,
            LeaveType = dto.LeaveType,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Description = dto.Description,
            Status = "Beklemede",
            CreatedAt = DateTime.Now
        };

        _context.LeaveRequests.Add(leaveRequest);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "İzin talebi başarıyla oluşturuldu.",
            leaveRequest.Id
        });
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateLeaveRequestStatus(int id, LeaveRequestUpdateStatusDto dto)
    {
        var allowedStatuses = new[] { "Beklemede", "Onaylandı", "Reddedildi" };

        if (!allowedStatuses.Contains(dto.Status))
        {
            return BadRequest("Geçersiz izin durumu. Durum Beklemede, Onaylandı veya Reddedildi olmalıdır.");
        }

        var leaveRequest = await _context.LeaveRequests.FindAsync(id);

        if (leaveRequest == null)
        {
            return NotFound("İzin kaydı bulunamadı.");
        }

        leaveRequest.Status = dto.Status;

        await _context.SaveChangesAsync();

        return Ok("İzin durumu başarıyla güncellendi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLeaveRequest(int id)
    {
        var leaveRequest = await _context.LeaveRequests.FindAsync(id);

        if (leaveRequest == null)
        {
            return NotFound("İzin kaydı bulunamadı.");
        }

        _context.LeaveRequests.Remove(leaveRequest);
        await _context.SaveChangesAsync();

        return Ok("İzin kaydı başarıyla silindi.");
    }
}