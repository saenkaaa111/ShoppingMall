using Microsoft.AspNetCore.Mvc;
using ShoppingMall.Business;
using ShoppingMall.Data;
using ShoppingMallBusiness;
using Swashbuckle.AspNetCore.Annotations;

namespace ShoppingMall.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitorsController : Controller
    {
        private readonly IVisitorsService _visitorService;

        public VisitorsController(IVisitorsService visitorService)
        {
            _visitorService = visitorService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get current visitor count")]
        [SwaggerResponse(StatusCodes.Status200OK)]        

        public async Task<ActionResult<int>> GetCurrentVisitorCount()
        {
            var result = await _visitorService.GetCurrentVisitorCountAsync();
            return Ok(result);            
        }
        
        [HttpPost("entry/{gate}")]
        [SwaggerOperation(Summary = "Register entry through the door")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<ActionResult<GatewayLogModel>> RegisterEntry(Gateway gate)
        {
            var result = await _visitorService.RegisterEntryAsync(gate);
            return Ok(result);
        }

        [HttpPost("exit/{gate}")]
        [SwaggerOperation(Summary = "Register exit through the door")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<ActionResult<GatewayLogModel?>> RegisterExit(Gateway gate)
        {
            var result = await _visitorService.RegisterExitAsync(gate);
            return Ok(result);
        }

        [HttpGet("statistics")]
        [SwaggerOperation(Summary = "Get visitor statistics for a period")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<VisitorStatisticsModel>>> GetStatistics(
        [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                return BadRequest("Start date cannot be after end date");

            var result = await _visitorService.GetVisitorStatisticsAsync(startDate, endDate);
            return Ok(result);
        }

    }
}

