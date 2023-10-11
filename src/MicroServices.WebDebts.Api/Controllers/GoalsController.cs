using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
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
    [Route("Goals")]
    [ApiController]
    public class GoalsController : ControllerBase
    {
        private readonly IGoalsApplicationService _goalsApplicationService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public GoalsController(IGoalsApplicationService goalsApplicationService, IHttpContextAccessor httpContextAccessor)
        {
            _goalsApplicationService = goalsApplicationService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost, Route("Create")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> CreateGoalAsync([FromBody] GoalAppModel goalAppModel)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var goal = await _goalsApplicationService.CreateGoal(goalAppModel, _userId);

            return new OkObjectResult(goal);
        }

        [HttpGet, Route("Filter")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> FilterGoalsAsync()
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var goals = await _goalsApplicationService.GetGoals(_userId);

            return new OkObjectResult(goals);
        }
    }
}
