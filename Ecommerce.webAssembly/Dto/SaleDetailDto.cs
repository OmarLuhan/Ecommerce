namespace Ecommerce.webAssembly.Dto;

public class SaleDetailDto
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal Total { get; set; }
}