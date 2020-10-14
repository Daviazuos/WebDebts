using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost, Route("CreateSimple")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> CreateSimpleAsync([FromBody] DebtsAppModel createDebtsRequest)
        {
            var debt = await _debtsApplicationService.CreateSimpleDebt(createDebtsRequest);

            return new OkObjectResult(debt);
        }

        [HttpGet, Route("GetSimple")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> GetSimpleAsync()
        {
            return new OkObjectResult(true);
        }
    }
}
