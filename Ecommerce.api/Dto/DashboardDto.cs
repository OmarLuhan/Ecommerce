namespace Ecommerce.api.Dto;

public class DashboardDto
{
    public string TotalIncome { get; set; } = null!;
    public int TotalSales { get; set; }
    public int TotalProducts { get; set; }
    public int TotalUsers { get; set; }
}