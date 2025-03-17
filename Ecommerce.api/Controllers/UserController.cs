using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Get User");
    }
}