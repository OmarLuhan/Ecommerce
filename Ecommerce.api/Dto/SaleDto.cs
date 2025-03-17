namespace Ecommerce.api.Dto;

public class SaleDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Total { get; set; }
    

    public virtual ICollection<SaleDetailDto> SaleDetails { get; set; } = [];
    
}