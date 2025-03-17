using System.ComponentModel.DataAnnotations;

namespace Ecommerce.api.Dto;

public class UserDto
{
    public int Id { get; set; }
    [Required]
    public string FullName { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public string ConfirmPassword { get; set; } = null!;

    public string Role { get; set; } = null!;
}