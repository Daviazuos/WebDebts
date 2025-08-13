using MicroServices.WebDebts.Application.Models.PlannerModels;
using MicroServices.WebDebts.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Api.Controllers
{
    [Route("planner")]
    [ApiController]
    public class PlannerController : ControllerBase
    {
        private readonly IPlannerService _plannerService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public PlannerController(IPlannerService plannerService, IHttpContextAccessor httpContextAccessor)
        {
            _plannerService = plannerService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("Create")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreatePlannerRequest request)
        {
            if (request == null)
                return BadRequest();

            var response = await _plannerService.AddPlannerAsync(request);
            return Ok(response);
        }

        [HttpPut("{id}/frequencies")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddFrequenciesAsync(Guid id, [FromBody] AddPlannerFrequenciesRequest request)
        {
            if (request == null)
                return BadRequest();

            await _plannerService.AddPlannerFrequenciesAsync(id, request);
            return NoContent();
        }

        [HttpPut("frequency/{plannerFrequencyId}/categories")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategoriesAsync(Guid plannerFrequencyId, [FromBody] AddPlannerCategoriesRequest request)
        {
            if (request == null)
                return BadRequest();

            await _plannerService.AddPlannerCategoriesAsync(plannerFrequencyId, request);
            return NoContent();
        }
    }
}
