using System.Net;
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
   public async Task<ActionResult<Response<List<ProductDto>>>> GetProducts(string? search = null)
   {
      var response = new Response<List<ProductDto>>();
      search??="";
      try
      {
         response.Status = HttpStatusCode.OK;
         response.Data = await service.ListAsync(search);
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

   [HttpGet("Catalog/{category}")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<Response<List<ProductDto>>>> GetCatalog(string category, string? search = null)
   {
      var response = new Response<List<ProductDto>>();
      search??="";
      try
      {
         response.Status = HttpStatusCode.OK;
         response.Data = await service.CatalogAsync(category, search);
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
   
   [HttpGet("Get/{id:int}")]
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
   public async Task<ActionResult<Response<ProductDto>>>AddProduct([FromBody] ProductDto product)
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
   public async Task<IActionResult> UpdateProduct([FromBody] ProductDto product)
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