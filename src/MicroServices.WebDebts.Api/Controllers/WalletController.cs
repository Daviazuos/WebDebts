using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Models.WalletModels;
using MicroServices.WebDebts.Application.Service;
using MicroServices.WebDebts.Domain.Models.Enum;
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
    [Route("Wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletApplicationService _walletApplicationService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public WalletController(IWalletApplicationService walletApplicationService, IHttpContextAccessor httpContextAccessor)
        {
            _walletApplicationService = walletApplicationService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost, Route("Create")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> CreateWalletAsync([FromBody] CreateWalletAppModel walletAppModel)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var waletId = await _walletApplicationService.CreateWallet(walletAppModel, _userId);

            return new OkObjectResult(waletId);
        }

        [HttpGet, Route("GetWalletById")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetWalletByIdResponse>> GetWalletById([FromQuery] Guid walletId)
        {
            var cardId = await _walletApplicationService.GetWalletById(walletId);

            return new OkObjectResult(cardId);
        }

        [HttpGet, Route("GetWallets")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetWalletByIdResponse>> GetWallets(WalletStatus walletStatus, int month, int year)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var wallet = await _walletApplicationService.GetWallets(walletStatus, month, year, _userId);

            return new OkObjectResult(wallet);
        }

        [HttpPut, Route("UpdateWallet")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> GetByIdAsync([FromQuery] Guid id, [FromBody] CreateWalletAppModel walletAppModel)
        {
            var walletId = await _walletApplicationService.UpdateWallet(id, walletAppModel);

            return new OkObjectResult(walletId);
        }

        [HttpPut, Route("UpdateWalletInstallment")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> UpdateWalletInstallmentAsync([FromQuery] Guid id, [FromBody] WalletInstallmentAppModel walletInstallmentAppModel)
        {
            var walletId = await _walletApplicationService.UpdateWalletInstallment(id, walletInstallmentAppModel);

            return new OkObjectResult(walletId);
        }

        [HttpDelete, Route("DeleteWallet")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteByIdAsync([FromQuery] Guid id)
        {
            await _walletApplicationService.DeleteWallet(id);

            return new OkResult();
        }

        [HttpGet, Route("GetResponsiblePartiesWallets")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<GetWalletResponsiblePartiesResponse>>> GetResponsiblePartiesWallets([FromQuery] int month, int year, Guid? responsiblePartyId = null)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var response = await _walletApplicationService.GetResponsiblePartiesWallets(responsiblePartyId, month, year, _userId);

            return new OkObjectResult(response);
        }
    }
}
