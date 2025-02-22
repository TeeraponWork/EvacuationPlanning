using EvacuationPlanning.Core.Dto.Vehicles;
using EvacuationPlanning.Core.Interfaces.IVehicles;
using Microsoft.AspNetCore.Mvc;

namespace EvacuationPlanning.Controllers.vehicles
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehiclesService _vehiclesService;
        public VehiclesController(IVehiclesService vehiclesService)
        {
            _vehiclesService = vehiclesService;
        }

        [HttpPost(Name = "AddVehicles")]
        public async Task<IActionResult> Add(VehiclesDto request)
        {
            try
            {
                var result = await _vehiclesService.Add(request);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
