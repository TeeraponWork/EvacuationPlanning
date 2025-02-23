using EvacuationPlanning.Core.Dto.EvacuationZones;
using EvacuationPlanning.Core.Interfaces.IEvacuationZones;
using Microsoft.AspNetCore.Mvc;

namespace EvacuationPlanning.Controllers.evacuation_zones
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvacuationZonesController : ControllerBase
    {
        private readonly IEvacuationZonesServices _evacuationZonesServices;
        public EvacuationZonesController(IEvacuationZonesServices evacuationZonesServices)
        {
            _evacuationZonesServices = evacuationZonesServices;
        }

        [HttpPost(Name = "AddEvacuation")]
        public async Task<IActionResult> Add(EvacuationZonesRequestDto request)
        {
            try
            {
                var result = await _evacuationZonesServices.Add(request);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
