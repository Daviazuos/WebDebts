using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Api.Controllers
{
    [Route("Debts")]
    [ApiController]
    public class DebtsController : ControllerBase
    {
        private readonly IDebtsApplicationService _debtsApplicationService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DebtsController(IDebtsApplicationService debtsApplicationService, IHttpContextAccessor httpContextAccessor)
        {
            _debtsApplicationService = debtsApplicationService;
            _httpContextAccessor = httpContextAccessor;

        }

        [HttpPost, Route("Create")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> CreateSimpleAsync([FromBody] CreateDebtAppModel createDebtsRequest)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var debt = await _debtsApplicationService.CreateDebt(createDebtsRequest, _userId);

            return new OkObjectResult(debt);
        }

        [HttpPut, Route("Edit")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EditDebtAsync([FromQuery] Guid id, [FromBody] CreateDebtAppModel createDebtsRequest)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            await _debtsApplicationService.EditDebt(id, createDebtsRequest, _userId);

            return new OkResult();
        }

        [HttpGet, Route("GetDebtById")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetDebtByIdResponse>> GetByIdAsync([FromQuery] GetDebtByIdRequest getDebtByIdRequest)
        {
            var debt = await _debtsApplicationService.GetDebtsById(getDebtByIdRequest.Id);

            return new OkObjectResult(debt); 
        }

        [HttpGet, Route("FilterDebt")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetDebtByIdResponse>> FilterAsync([FromQuery] FilterDebtRequest filterDebtRequest)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var debt = await _debtsApplicationService.FilterDebtsById(filterDebtRequest, _userId);

            return new OkObjectResult(debt);
        }

        [HttpGet, Route("FilterInstallments")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> FilterInstallmentsAsync([FromQuery] FilterInstallmentsRequest filterInstallmentsRequest)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var installments = await _debtsApplicationService.FilterInstallments(filterInstallmentsRequest, _userId);

            return new OkObjectResult(installments);
        }

        [HttpPut, Route("InstallmentsStatus")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutInstallmentsAsync([FromQuery] PutInstallmentsRequest putInstallmentsRequest)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            await _debtsApplicationService.PutInstallments(putInstallmentsRequest, _userId);

            return new NoContentResult();
        }

        [HttpDelete, Route("Delete")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteAsync([FromQuery] DeleteDebtByIdRequest deletePersonRequest)
        {
            await _debtsApplicationService.DeletePerson(deletePersonRequest);

            return NoContent();
        }

        [HttpGet, Route("GetSumByMonth")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<GetSumbyMonthResponse>>> GetSumByMonth([FromQuery] GetSumByMonthRequest getSumByMonthRequest)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var response = await _debtsApplicationService.GetSumByMonth(getSumByMonthRequest, _userId);

            return new OkObjectResult(response);
        }
    }
}
