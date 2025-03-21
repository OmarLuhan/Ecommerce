using System.Net;
using Ecommerce.api.Dto;
using Ecommerce.api.Helpers;
using Ecommerce.api.Service;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.api.Controllers;

[Route("api/[controller]")]
public class CategoryController(ICategoryService service) : ControllerBase
{
    [HttpGet("list/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<List<CategoryDto>>>> ListAsync(string? search = null)
    {
        var response = new Response<List<CategoryDto>>();
        search ??= "";
        try
        {
            response.Status = HttpStatusCode.OK;
            response.Success = true;
            response.Data = await service.ListAsync(search);
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
    [HttpGet("get/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<CategoryDto>>> Get(int id)
    {
        var response = new Response<CategoryDto>();
        try
        {
            response.Status = HttpStatusCode.OK;
            response.Success = true;
            response.Data = await service.GetCategoryAsync(id);
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
    public async Task<ActionResult<Response<CategoryDto>>> AddAsync([FromBody] CategoryDto category)
    {
        var response = new Response<CategoryDto>();
        try
        {
            response.Status = HttpStatusCode.Created;
            response.Success = true;
            response.Data = await service.CreateAsync(category);
            return CreatedAtAction(nameof(Get), new { id = response.Data.Id }, response);
        }
        catch (Exception ex)
        {
            response.Status = HttpStatusCode.InternalServerError;
            response.Message = ex.Message;
            response.Success = false;
            return StatusCode(500, response);
        }
    }
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<bool>>> UpdateAsync([FromBody] CategoryDto category)
    {
        var response = new Response<bool>();
        try
        {
            response.Data= await service.UpdateAsync(category);
            response.Status = HttpStatusCode.OK;
            response.Success = true;
            
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
    [HttpDelete("delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<bool>>> DeleteAsync(int id)
    {
        var response = new Response<bool>();
        try
        {
            response.Status = HttpStatusCode.NoContent;
            response.Success = true;
            response.Data= await service.DeleteAsync(id);
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
    
}