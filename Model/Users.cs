using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exemplo02.Model;

[Table("users")]
public class Users
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password_hash { get; set; } = string.Empty;

    public string? Name { get; set; }

    public bool Active { get; set; }

    public DateTime Created_at { get; set; }

    public DateTime Updated_at { get; set; }
}