using System.Net;
using Ecommerce.api.Dto;
using Ecommerce.api.Helpers;
using Ecommerce.api.Service;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.api.Controllers;


[Route("api/[controller]")]
public class DashboardController(IDashboardService service):ControllerBase{
    
    [HttpGet("Summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Response<DashboardDto>>>Summary(){
        var response = new Response<DashboardDto>();
        try{
            response.Status=HttpStatusCode.OK;
            response.Data= await service.SummaryAsync();
            response.Success=true;
            return Ok(response);
        }catch(Exception ex){
            response.Success=false;
            response.Status=HttpStatusCode.InternalServerError;
            response.Message = ex.Message;
            return StatusCode(500,response);
        }
    }
}