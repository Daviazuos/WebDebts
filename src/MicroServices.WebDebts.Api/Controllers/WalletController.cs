using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Service;
using MicroServices.WebDebts.Domain.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Api.Controllers
{
    [Route("Wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletApplicationService _walletApplicationService;

        public WalletController(IWalletApplicationService walletApplicationService)
        {
            _walletApplicationService = walletApplicationService;
        }

        [HttpPost, Route("Create")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> CreateWalletAsync([FromBody] WalletAppModel walletAppModel)
        {
            var waletId = await _walletApplicationService.CreateWallet(walletAppModel);

            return new OkObjectResult(waletId);
        }

        [HttpGet, Route("GetWalletById")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetWalletByIdResponse>> GetWalletById([FromQuery] Guid walletId)
        {
            var cardId = await _walletApplicationService.GetWalletById(walletId);

            return new OkObjectResult(cardId);
        }

        [HttpGet, Route("GetWallets")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetWalletByIdResponse>> GetWallets(WalletStatus walletStatus)
        {
            var wallet = await _walletApplicationService.GetWallets(walletStatus);

            return new OkObjectResult(wallet);
        }

        [HttpPut, Route("UpdateWallet")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> GetByIdAsync([FromQuery] Guid id, [FromBody] WalletAppModel walletAppModel)
        {
            var walletId = await _walletApplicationService.UpdateWallet(id, walletAppModel);

            return new OkObjectResult(walletId);
        }

        [HttpDelete, Route("DeleteWallet")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteByIdAsync([FromQuery] Guid id)
        {
            await _walletApplicationService.DeleteWallet(id);

            return new OkResult();
        }
    }
}
