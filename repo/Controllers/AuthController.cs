using HumanResourcesApi.DTOs;
using HumanResourcesApi.Models;
using HumanResourcesApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HumanResourcesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TokenService _tokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        TokenService tokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
        await EnsureRolesAsync();

        if (string.IsNullOrWhiteSpace(dto.FullName))
        {
            return BadRequest("Ad soyad boş olamaz.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest("E-posta boş olamaz.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest("Şifre boş olamaz.");
        }

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);

        if (existingUser != null)
        {
            return BadRequest("Bu e-posta adresiyle kayıtlı bir kullanıcı zaten var.");
        }

        var isFirstUser = !await _userManager.Users.AnyAsync();

        var user = new ApplicationUser
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim(),
            UserName = dto.Email.Trim()
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(errors);
        }

        var assignedRole = isFirstUser ? "Admin" : "Personel";

        var roleResult = await _userManager.AddToRoleAsync(user, assignedRole);

        if (!roleResult.Succeeded)
        {
            var errors = roleResult.Errors.Select(e => e.Description).ToList();
            return BadRequest(errors);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.CreateToken(user, roles);

        return Ok(new AuthResponseDto
        {
            Message = "Kullanıcı kaydı başarılı.",
            Email = user.Email ?? string.Empty,
            Role = roles.FirstOrDefault() ?? string.Empty,
            Roles = roles.ToList(),
            Token = token
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        await EnsureRolesAsync();

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest("E-posta boş olamaz.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest("Şifre boş olamaz.");
        }

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            return Unauthorized("E-posta veya şifre hatalı.");
        }

        var passwordCheck = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!passwordCheck)
        {
            return Unauthorized("E-posta veya şifre hatalı.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Count == 0)
        {
            var defaultRole = "IK";
            var roleResult = await _userManager.AddToRoleAsync(user, defaultRole);

            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => e.Description).ToList();
                return BadRequest(errors);
            }

            roles = await _userManager.GetRolesAsync(user);
        }

        var token = _tokenService.CreateToken(user, roles);

        return Ok(new AuthResponseDto
        {
            Message = "Giriş başarılı.",
            Email = user.Email ?? string.Empty,
            Role = roles.FirstOrDefault() ?? string.Empty,
            Roles = roles.ToList(),
            Token = token
        });
    }

    private async Task EnsureRolesAsync()
    {
        var roles = new[] { "Admin", "IK", "Personel" };

        foreach (var role in roles)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}