using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Api.Controllers
{
    [Route("Debts")]
    [ApiController]
    public class DebtsController : ControllerBase
    {
        private readonly IDebtsApplicationService _debtsApplicationService;

        public DebtsController(IDebtsApplicationService debtsApplicationService)
        {
            _debtsApplicationService = debtsApplicationService;
        }

        [HttpPost, Route("Create")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> CreateSimpleAsync([FromBody] DebtsAppModel createDebtsRequest)
        {
            var debt = await _debtsApplicationService.CreateDebt(createDebtsRequest);

            return new OkObjectResult(debt);
        }

        [HttpGet, Route("GetDebtById")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetDebtByIdResponse>> GetByIdAsync([FromQuery] GetDebtByIdRequest getDebtByIdRequest)
        {
            var debt = await _debtsApplicationService.GetDebtsById(getDebtByIdRequest.Id);

            return new OkObjectResult(debt); 
        }

        [HttpGet, Route("FilterDebt")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetDebtByIdResponse>> FilterAsync([FromQuery] FilterDebtRequest filterDebtRequest)
        {
            var debt = await _debtsApplicationService.FilterDebtsById(filterDebtRequest);

            return new OkObjectResult(debt);
        }

        [HttpDelete, Route("Delete")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteAsync([FromQuery] DeleteDebtByIdRequest deletePersonRequest)
        {
            await _debtsApplicationService.DeletePerson(deletePersonRequest);

            return NoContent();
        }
    }
}
