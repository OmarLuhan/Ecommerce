namespace Ecommerce.webAssembly.Dto;

public class CarrDto
{
    public ProductDto? Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }
    
}