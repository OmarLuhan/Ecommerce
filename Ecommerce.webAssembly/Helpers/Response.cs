using System.Net;

namespace Ecommerce.webAssembly.Helpers;

public class Response<T>
{
    public HttpStatusCode Status { get; set; }
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}
