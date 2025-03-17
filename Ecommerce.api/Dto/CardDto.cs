using System.ComponentModel.DataAnnotations;

namespace Ecommerce.api.Dto;

public class CardDto
{
    [Required]
    public string Holder { get; set; } = null!;
    [Required]
    public string Number { get; set; } = null!;
    [Required]
    public string Expiration { get; set; } = null!;
    [Required]
    public string Cvv { get; set; } = null!;
}