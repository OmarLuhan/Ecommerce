using System.ComponentModel.DataAnnotations;

namespace Ecommerce.api.Helpers;
public class SpecParam
{
    [Range(1, int.MaxValue)] 
    public int PageNumber { get; set; } = 1; 
    
    [Range(1, 100)] 
    public int PageSize { get; set; } = 20;
    
    public string? SortBy { get; set; } ="Id"; 
    public bool SortDesc { get; set; } = false;
}