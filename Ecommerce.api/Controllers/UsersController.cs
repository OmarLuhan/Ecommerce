using System.Net;
using System.Text.Json;
using Ecommerce.api.Dto;
using Ecommerce.api.Helpers;
using Ecommerce.api.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService service) : ControllerBase
{
    [HttpGet("{role:alpha}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<IEnumerable<UserDto>>>> List(
        string role,
        [FromQuery] SpecParam? specParams,  
        [FromQuery] string? search = null)
    {
        var response = new Response<IEnumerable<UserDto>>();
        try
        {
            specParams ??= new SpecParam();
            
            var pagedData = await service.ListAsync(specParams, role, search ?? "");
            
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(new
            {
                pagedData.MetaData.CurrentPage,
                pagedData.MetaData.PageSize,
                pagedData.MetaData.TotalCount,
                pagedData.MetaData.TotalPages,
                HasPrevious = pagedData.MetaData.CurrentPage > 1,
                HasNext = pagedData.MetaData.CurrentPage < pagedData.MetaData.TotalPages
            }));
            
            response.Status = HttpStatusCode.OK;
            response.Success = true;
            response.Data = pagedData;

            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Status = HttpStatusCode.InternalServerError;
            response.Success = false;
            response.Message = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }
    [HttpGet("get/{id:int}")]
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
            response.Status = HttpStatusCode.InternalServerError;
            response.Message = ex.Message;
            response.Success = false;
            return StatusCode(500, response);
        }
    }
    [HttpPost("add")]
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

    [HttpPost("authorize")]
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
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<bool>>> Update([FromBody] UserDto user)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = HttpStatusCode.NoContent;
            response.Success = true;
            response.Data=await service.UpdateAsync(user);
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message=ex.Message;
            response.Status= HttpStatusCode.InternalServerError;
            return StatusCode(500, response);
        }
    }
    
    [HttpDelete("delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<bool>>> Delete(int id)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = HttpStatusCode.NoContent;
            response.Success = true;
            response.Data=await service.DeleteAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Message=ex.Message;
            response.Success = false;
            response.Status= HttpStatusCode.InternalServerError;
            return StatusCode(500, response);
        }
    }
}