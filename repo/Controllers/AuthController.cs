using HumanResourcesApi.DTOs;
using HumanResourcesApi.Models;
using HumanResourcesApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TokenService _tokenService;

    public AuthController(UserManager<ApplicationUser> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
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

        var token = _tokenService.CreateToken(user);

        return Ok(new AuthResponseDto
        {
            Message = "Kullanıcı kaydı başarılı.",
            Email = user.Email ?? string.Empty,
            Token = token
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
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

        var token = _tokenService.CreateToken(user);

        return Ok(new AuthResponseDto
        {
            Message = "Giriş başarılı.",
            Email = user.Email ?? string.Empty,
            Token = token
        });
    }
}