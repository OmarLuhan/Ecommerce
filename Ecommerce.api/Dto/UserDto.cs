using System.ComponentModel.DataAnnotations;

namespace Ecommerce.api.Dto;

public class UserDto
{
    public int Id { get; init; }
    public string FullName { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public string Password { get; set; } = null!;

    public string Role { get; init; } = null!;
}
public class UserCreateDto
{
    [Required]
    public string FullName { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public string ConfirmPassword { get; set; } = null!;
    [Required]
    public string Role { get; set; } = null!;
}
public class UserUpdateDto
{
    public int Id { get; init; }
    [Required]
    public string FullName { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}