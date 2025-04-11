using System.Net;
using System.Text.Json;
using Ecommerce.api.Dto;
using Ecommerce.api.Helpers;
using Ecommerce.api.Service;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.api.Controllers;

[Route("api/[controller]")]
public class CategoriesController(ICategoryService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<IEnumerable<CategoryDto>>>> ListAsync([FromQuery] SpecParam? specParam,string? search = null)
    {
        var response = new Response<IEnumerable<CategoryDto>>();
        try
        {
            PageList<CategoryDto>pagedData=await service.ListAsync(specParam,search??"");
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
            response.Message = ex.Message;
            response.Success = false;
            return StatusCode(500, response);
        }
    }
    [HttpGet("{id:int}")]
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
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Response<CategoryDto>>> AddAsync([FromBody] CategoryCreateDto category)
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
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync([FromBody] CategoryUpdateDto category)
    {
        try
        {
            await service.UpdateAsync(category);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = HttpStatusCode.InternalServerError,
                ex.Message,
                Success = false
            });
        }
    }
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            await service.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, new
            {
                Status = HttpStatusCode.InternalServerError,
                ex.Message,
                Success = false
            });
        }
    }
    
}