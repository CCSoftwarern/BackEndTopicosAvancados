using exemplo02.Data;
using exemplo02.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace exemplo02.Controllers;

[Route("api/[controller]")]
[ApiController]

public class UsersController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Users>> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

        [HttpPost]
        public async Task<ActionResult<Users>> PostUser([FromBody] Users user)
        {
            user.Password_hash = BCrypt.Net.BCrypt.HashPassword(user.Password_hash);
            user.Created_at = DateTime.UtcNow;
            user.Updated_at = DateTime.UtcNow;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new
            {
                user.Id,
                user.Email,
                user.Name,
                user.Active,
                user.Created_at
            });
        }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, [FromBody] Users user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        var existingUser = await context.Users.FindAsync(id);

        if (existingUser == null)
        {
            return NotFound();
        }

        existingUser.Email = user.Email;
        existingUser.Password_hash = user.Password_hash;
        existingUser.Name = user.Name;
        existingUser.Active = user.Active;
        existingUser.Updated_at = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return NoContent();
    }

    
}