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
    [Route("SpendingCeiling")]
    [ApiController]
    public class SpendingCeilingController : ControllerBase
    {
        private readonly ISpendingCeilingApplicationService _spendingCeilingApplicationService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public SpendingCeilingController(ISpendingCeilingApplicationService spendingCeilingApplicationService, IHttpContextAccessor httpContextAccessor)
        {
            _spendingCeilingApplicationService = spendingCeilingApplicationService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost, Route("Create")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> CreateWalletAsync([FromBody] SpendingCeilingAppModel spendingCeilingAppModel)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var spendingCeiling = await _spendingCeilingApplicationService.CreateSpendingCeiling(spendingCeilingAppModel, _userId);

            return new OkObjectResult(spendingCeiling);
        }

        [HttpGet, Route("GetByMonth")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetWalletByIdResponse>> GetByMonth(int month, int year)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var spendingCeiling = await _spendingCeilingApplicationService.GetSpendingCeiling(_userId, month, year);

            return new OkObjectResult(spendingCeiling);
        }
    }
}
