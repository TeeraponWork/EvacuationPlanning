using EvacuationPlanning.Core.Dto.Plan;
using EvacuationPlanning.Core.Interfaces.IPlan;
using Microsoft.AspNetCore.Mvc;

namespace EvacuationPlanning.Controllers.evacuations
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvacuationsController : ControllerBase
    {
        private readonly IPlanServices _planServices;
        public EvacuationsController(IPlanServices planServices)
        {
            _planServices = planServices;
        }
        [HttpPost("plan", Name = "Plan")]
        public async Task<IActionResult> Plan()
        {
            try
            {
                var result = await _planServices.EvacuationScheduler();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("status", Name = "status")]
        public async Task<IActionResult> Status()
        {
            try
            {
                var result = await _planServices.StatusEvacuation();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("update", Name = "update")]
        public async Task<IActionResult> Update(UpdatePlanDto model)
        {
            try
            {
                var result = await _planServices.UpdateEvacuation(model);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("delete", Name = "delete")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var result = await _planServices.DeleteEvacuation();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
