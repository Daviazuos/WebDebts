using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Models.WalletModels;
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
        public async Task<ActionResult> EditDebtAsync([FromQuery] Guid id, [FromBody] UpdateDebtAppModel createDebtsRequest)
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


        [HttpGet, Route("GetCategories")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<GetCategoryRequest>>> GetCategories()
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var response = await _debtsApplicationService.GetCategories(_userId);

            return new OkObjectResult(response);
        }

        [HttpGet, Route("GetDebtCategories")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<GetDebtCategoryResponse>>> GetDebtCategories([FromQuery] FilterDebtsCategoriesRequest filterDebtsCategoriesRequest)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var response = await _debtsApplicationService.GetDebtCategories(filterDebtsCategoriesRequest, _userId);

            return new OkObjectResult(response);
        }

        [HttpPost, Route("CreateCategory")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> CreateCategory([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var category = await _debtsApplicationService.CreateCategory(createCategoryRequest, _userId);

            return new OkObjectResult(category);
        }

        [HttpGet, Route("GetAnaliticsByMonth")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetAnaliticsResponse>> GetAnaliticsByMonth([FromQuery] GetAnaliticsRequest getAnaliticsRequest)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var response = await _debtsApplicationService.GetAnaliticsByMonth(getAnaliticsRequest, _userId);

            return new OkObjectResult(response);
        }

        [HttpPut, Route("Installments")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EditInstallmentsAsync([FromQuery] Guid id, InstallmentsAppModel installmentsAppModel)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            await _debtsApplicationService.EditInstallments(id, installmentsAppModel, _userId);

            return new NoContentResult();
        }

        [HttpDelete, Route("DeleteInstallment")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteInstallmentAsync([FromQuery] Guid id)
        {
            await _debtsApplicationService.DeleteInstallment(id);

            return NoContent();
        }

        [HttpGet, Route("GetResponsiblePartiesDebts")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<GetDebtResponsiblePartiesResponse>>> GetResponsiblePartiesDebts([FromQuery] Guid responsiblePartyId, int month, int year)
        {
            var response = await _debtsApplicationService.GetResponsiblePartiesDebts(responsiblePartyId, month, year);

            return new OkObjectResult(response);
        }
    }
}
