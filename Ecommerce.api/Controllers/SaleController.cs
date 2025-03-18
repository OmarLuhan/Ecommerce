using System.Net;
using Ecommerce.api.Dto;
using Ecommerce.api.Helpers;
using Ecommerce.api.Service;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.api.Controllers;
[Route("api/[controller]")]
public class SaleController(ISaleService service):ControllerBase
{
   [HttpGet("Get/{id:int}")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public async Task<ActionResult<Response<SaleDto>>> Get(int id)
   {
      var response = new Response<SaleDto>();
      try
      {
         response.Status = HttpStatusCode.OK;
         response.Data = await service.GetSaleAsync(id);
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
 public async Task<ActionResult<Response<SaleDto>>> AddSale([FromBody] SaleDto sale)
 {
    var response = new Response<SaleDto>();
    try
    {
       response.Status = HttpStatusCode.Created;
       response.Data = await service.RegisterSaleAsync(sale);
       response.Success = true;
       return CreatedAtAction(nameof(Get), response);
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