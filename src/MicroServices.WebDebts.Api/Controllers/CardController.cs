using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Api.Controllers
{
    [Route("Card")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardsApplicationService _cardsApplicationService;

        public CardController(ICardsApplicationService cardsApplicationService)
        {
            _cardsApplicationService = cardsApplicationService;
        }

        [HttpPost, Route("Create")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> CreateCardAsync([FromBody] CardAppModel cardAppModel)
        {
            var cardId = await _cardsApplicationService.CreateCard(cardAppModel);

            return new OkObjectResult(cardId);
        }

        [HttpPost, Route("AddValues")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenericResponse>> AddValuesCardAsync([FromBody] CreateDebtAppModel createDebtAppModel, Guid CardId)
        {
            var cardId = await _cardsApplicationService.AddValuesCard(createDebtAppModel, CardId);

            return new OkObjectResult(cardId);
        }

        [HttpGet, Route("GetCardById")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetDebtByIdResponse>> GetByIdAsync([FromQuery] GetDebtByIdRequest getDebtByIdRequest)
        {
            var card = await _cardsApplicationService.GetCardById(getDebtByIdRequest.Id);

            return new OkObjectResult(card);
        }

        [HttpGet, Route("FilterCards")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<GetCardsResponse>>> FilterCardAsync([FromQuery] GetCardByIdRequest getCardByIdRequest)
        {
            var card = await _cardsApplicationService.FilterCardsAsync(getCardByIdRequest.Id, getCardByIdRequest.Month, getCardByIdRequest.Year);

            return new OkObjectResult(card);
        }
    }
}
