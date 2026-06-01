using LeadXTechnologiesApi.Data;
using LeadXTechnologiesApi.Models;
using LeadXTechnologiesApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LeadXTechnologiesApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var user = _context.Users.FirstOrDefault(x => x.Email == dto.Email);

        if (user == null)
            return Unauthorized("Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials");

        var jwt = _config.GetSection("Jwt");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name ?? "")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            jwt["Issuer"],
            jwt["Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(int.Parse(jwt["ExpireMinutes"]!)),
            signingCredentials: creds
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            user = new { user.Email, user.Name }
        });
    }
}