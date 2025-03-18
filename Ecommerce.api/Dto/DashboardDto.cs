namespace Ecommerce.api.Dto;

public class DashboardDto
{
    public string Income { get; set; } = null!;
    public int Sales { get; set; }
    public int Products { get; set; }
    public int Customers { get; set; }
}