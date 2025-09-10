using MicroServices.WebDebts.Application.Models.PlannerModels;
using MicroServices.WebDebts.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Security.Claims;
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

            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var response = await _plannerService.AddPlannerAsync(request, userId);
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

            var response = await _plannerService.AddPlannerCategoriesAsync(plannerFrequencyId, request);
            return Ok(response);
        }

        [HttpGet("user/month/{month}/year/{year}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByUserAndMonthAsync(int month, int year)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            if (userId == Guid.Empty || month < 1 || month > 12 || year < 1)
                return BadRequest();
            var response = await _plannerService.GetPlannersByUserAndMonthAsync(userId, month, year);
            return Ok(response);
        }

        [HttpPut("category/{plannerCategoryId}/budget")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePlannerCategoryBudgetAsync(Guid plannerCategoryId, [FromBody] UpdatePlannerCategoryBudgetRequest request)
        {
            if (request == null)
                return BadRequest();

            var response = await _plannerService.UpdatePlannerCategoryBudgetAsync(plannerCategoryId, request.BudgetedValue);
            return Ok(response);
        }

        [HttpDelete("category/{plannerCategoryId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePlannerCategoryAsync(Guid plannerCategoryId)
        {
            if (plannerCategoryId == Guid.Empty)
                return BadRequest();

            await _plannerService.DeletePlannerCategoryAsync(plannerCategoryId);
            return NoContent();
        }

        [HttpPut("frequency/{plannerFrequencyId}/dates")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePlannerFrequencyDatesAsync(Guid plannerFrequencyId, [FromBody] UpdatePlannerFrequencyDatesRequest request)
        {
            if (request == null)
                return BadRequest();

            var response = await _plannerService.UpdatePlannerFrequencyDatesAsync(plannerFrequencyId, request.Start, request.End);
            return Ok(response);
        }

        [HttpDelete("frequency/{plannerFrequencyId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePlannerFrequencyAsync(Guid plannerFrequencyId)
        {
            if (plannerFrequencyId == Guid.Empty)
                return BadRequest();

            await _plannerService.DeletePlannerFrequencyAsync(plannerFrequencyId);
            return NoContent();
        }


    }
}
