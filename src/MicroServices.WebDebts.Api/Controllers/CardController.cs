using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Service;
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
    [Route("Card")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardsApplicationService _cardsApplicationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CardController(ICardsApplicationService cardsApplicationService, IHttpContextAccessor httpContextAccessor)
        {
            _cardsApplicationService = cardsApplicationService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost, Route("Create")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> CreateCardAsync([FromBody] CardAppModel cardAppModel)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var cardId = await _cardsApplicationService.CreateCard(cardAppModel, _userId);

            return new OkObjectResult(cardId);
        }

        [HttpPut, Route("Edit")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EditCardDebtsAsync([FromQuery] Guid id, CardAppModel cardAppModel)
        {
            await _cardsApplicationService.EditCardAsync(cardAppModel, id);

            return new NoContentResult();
        }

        [HttpPost, Route("AddValues")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> AddValuesCardAsync([FromBody] CreateDebtAppModel createDebtAppModel, Guid CardId)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var cardId = await _cardsApplicationService.AddValuesCard(createDebtAppModel, CardId, _userId);

            return new OkObjectResult(cardId);
        }

        [HttpGet, Route("GetCardById")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetDebtByIdResponse>> GetByIdAsync([FromQuery] GetDebtByIdRequest getDebtByIdRequest)
        {
            var card = await _cardsApplicationService.GetCardById(getDebtByIdRequest.Id);

            return new OkObjectResult(card);
        }

        [HttpGet, Route("FilterCards")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<GetCardsResponse>>> FilterCardAsync([FromQuery] GetCardByIdRequest getCardByIdRequest)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            var card = await _cardsApplicationService.FilterCardsAsync(getCardByIdRequest.Id, getCardByIdRequest.Month, getCardByIdRequest.Year, _userId);

            return new OkObjectResult(card);
        }

        [HttpPut, Route("PayCard")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PayCardDebtsAsync([FromQuery] PayCardResponseModel payCardResponseModel)
        {
            var _userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid));

            await _cardsApplicationService.PayCardDebtsAsync(payCardResponseModel, _userId);

            return new NoContentResult();
        }

        [HttpDelete, Route("DeleteCard")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteCard([FromQuery] Guid cardId)
        {
            await _cardsApplicationService.DeleteCardAsync(cardId);

            return new OkResult();
        }
    }
}
