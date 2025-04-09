using System.Net;
using System.Text.Json;
using Ecommerce.api.Dto;
using Ecommerce.api.Helpers;
using Ecommerce.api.Service;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.api.Controllers;

[Route("api/[controller]")]
public class ProductsController(IProductService service) : ControllerBase
{
   [HttpGet]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<Response<PageList<ProductDto>>>> GetProducts([FromQuery] SpecParam? specParams, [FromQuery] string? search = null)
   {
      var response = new Response<PageList<ProductDto>>();
      try
      {
         PageList<ProductDto>pagedData=await  service.ListAsync(specParams, search?? "");
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
         response.Data = pagedData;
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

   [HttpGet("catalog/{category}")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<Response<List<ProductDto>>>> GetCatalog(
      string category, 
      [FromQuery] SpecParam? specParams,
      [FromQuery]string? search = null)
   {
      var response = new Response<PageList<ProductDto>>();
      try
      {
         PageList<ProductDto> pagedData = await service.CatalogAsync(specParams, category, search ?? "");
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
         response.Data = pagedData;
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
   
   [HttpGet("{id:int}")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<Response<ProductDto>>> Get(int id)
   {
      var response = new Response<ProductDto>();
      try
      {
         response.Status = HttpStatusCode.OK;
         response.Data = await service.GetProductAsync(id);
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
   [HttpPost("Add")]
   [ProducesResponseType(StatusCodes.Status201Created)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<Response<ProductDto>>>AddProduct([FromBody] ProductCreateDto product)
   {
      var response = new Response<ProductDto>();
      try
      {
         response.Status = HttpStatusCode.Created;
         response.Data = await service.CreateAsync(product);
         response.Success = true;
         return CreatedAtAction(nameof(Get),new { id = response.Data.Id } ,response);
      }
      catch (Exception ex)
      {
         response.Status = HttpStatusCode.InternalServerError;
         response.Message = ex.Message;
         response.Success = false;
         return StatusCode(500, response);
      }
   }
   [HttpPut("Update")]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateDto product)
   {
      try
      {
         await service.UpdateAsync(product);
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
   [HttpDelete("Delete/{id:int}")]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<IActionResult> DeleteProduct(int id)
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