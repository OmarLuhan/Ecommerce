using System.Net;
using Ecommerce.api.Dto;
using Ecommerce.api.Helpers;
using Ecommerce.api.Service;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService service) : ControllerBase
{
    [HttpGet("List/{role:alpha}/{search:alpha?}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<List<UserDto>>>> List(string role, string search = "NA")
    {
        var response = new Response<List<UserDto>>();
        try
        {
            if (search == "NA") search = "";
            response.Status= HttpStatusCode.OK;
            response.Success = true;
            response.Data = await service.ListAsync(role, search);
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            response.Success = false;
            return StatusCode(500, response);
        }
    }
    [HttpGet("Get/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<UserDto>>> Get(int id)
    {
        var response = new Response<UserDto>();
        try
        {
            response.Status= HttpStatusCode.OK;
            response.Success = true;
            response.Data = await service.GetUserAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            response.Success = false;
            return StatusCode(500, response);
        }
    }
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<UserDto>>> Add([FromBody]UserDto user)
    {
        var response = new Response<UserDto>();
        try
        {
            response.Status= HttpStatusCode.Created;
            response.Success = true;
            response.Data = await service.CreateAsync(user);
            return CreatedAtAction(nameof(Get), new { id = response.Data.Id }, response);
        }
        catch (Exception ex)
        {
            response.Status= HttpStatusCode.InternalServerError;
            response.Message = ex.Message;
            response.Success = false;
            return StatusCode(500, response);
        }
    }

    [HttpPost("Authorize")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<SessionDto>>> Authorize([FromBody] LoginDto login)
    {
        var response = new Response<SessionDto>();
        try
        {
            response.Status= HttpStatusCode.OK;
            response.Success = true;
            response.Data = await service.AuthorizeAsync(login);
            return Ok(response);

        }
        catch (Exception ex)
        {
            response.Status= HttpStatusCode.InternalServerError;
            response.Message = ex.Message;
            response.Success = false;
            return StatusCode(500, response);
        }
    }
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] UserDto user)
    {
        try
        {
            _ =await service.UpdateAsync(user);
            return NoContent();
        }
        catch (Exception ex)
        {
            var response = new Response<UserDto>
            {
                Status = HttpStatusCode.InternalServerError,
                Message = ex.Message,
                Success = false
            };
            return StatusCode(500, response);
        }
    }
    
    [HttpDelete("Delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _ =await service.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            var response = new Response<UserDto>
            {
                Status = HttpStatusCode.InternalServerError,
                Message = ex.Message,
                Success = false
            };
            return StatusCode(500, response);
        }
    }
}