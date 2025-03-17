namespace Ecommerce.api.Helpers;

public class Response<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}
