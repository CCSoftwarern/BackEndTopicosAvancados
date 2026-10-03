using exemplo02.Data;
using exemplo02.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace exemplo02.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AppDbContext context) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Active);

        if (user == null)
        {
            return Unauthorized(new { message = "E-mail ou senha inválidos." });
        }

        bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password_hash);

        if (!isValid)
        {
            return Unauthorized(new { message = "E-mail ou senha inválidos." });
        }

        // Não retornar password_hash
        var result = new
        {
            id = user.Id,
            email = user.Email,
            name = user.Name,
            active = user.Active
        };

        return Ok(result);
    }
}