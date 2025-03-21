using System.ComponentModel.DataAnnotations;

namespace Ecommerce.webAssembly.Dto;
public class ProductDto
{
    public int Id { get; set; }
[Required]
    public string Name { get; set; } = null!;
    [Required]
    public string? Description { get; set; }
    [Required]
    public int? CategoryId { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public decimal OfferPrice { get; set; }
    [Required]
    public int Stock { get; set; }
    [Required]
    public string? Image { get; set; }
}