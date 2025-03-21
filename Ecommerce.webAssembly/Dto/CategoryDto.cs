using System.ComponentModel.DataAnnotations;

namespace Ecommerce.webAssembly.Dto;

public class CategoryDto
{
    public int Id { get; set; }
[Required]
    public string Name { get; set; } = null!;
}