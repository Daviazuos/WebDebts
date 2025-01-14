using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using MicroServices.WebDebts.Application.Models.ResponsiblePartyModels;

namespace MicroServices.WebDebts.Api.Controllers
{
    [Route("ResponsibleParty")]
    [ApiController]
    public class ResponsiblePartyController : ControllerBase
    {
        private readonly IResponsiblePartyService _responsiblePartyService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResponsiblePartyController(IResponsiblePartyService responsiblePartyService, IHttpContextAccessor httpContextAccessor)
        {
            _responsiblePartyService = responsiblePartyService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost, Route("Create")]
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> CreateResponsiblePartyAsync([FromBody] ResponsiblePartyAppModel responsiblePartyAppModel)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            await _responsiblePartyService.CreateResponsibleParty(responsiblePartyAppModel, _userId);

            return new OkResult();
        }

        [HttpPut, Route("Link")]
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> LinkResponsiblePartyAsync([FromQuery] Guid responsiblePartyId, Guid? debtId, Guid? walletId)
        {
            await _responsiblePartyService.LinkResponsiblePartyWDebt(responsiblePartyId, debtId, walletId);

            return new OkResult();
        }

        [HttpGet, Route("GetByUserId")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetByUserId()
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var responsibleParty = await _responsiblePartyService.GetResponsiblePartyByUserId(_userId);

            return new OkObjectResult(responsibleParty);
        }

    }
}
