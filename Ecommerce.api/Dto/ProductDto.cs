namespace Ecommerce.api.Dto;

public class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
    public string? CategoryName { get; set; }
    public decimal Price { get; set; }

    public decimal OfferPrice { get; set; }

    public int Stock { get; set; }

    public string? Image { get; set; }
}
public class ProductCreateDto
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? CategoryId { get; set; }
    
    public decimal Price { get; set; }

    public decimal OfferPrice { get; set; }

    public int Stock { get; set; }

    public string? Image { get; set; }
}
public class ProductUpdateDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? CategoryId { get; set; }
    
    public decimal Price { get; set; }

    public decimal OfferPrice { get; set; }

    public int Stock { get; set; }

    public string? Image { get; set; }
}