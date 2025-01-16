using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Models.Mappers;
using MicroServices.WebDebts.Application.Services;
using MicroServices.WebDebts.Domain.Common;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Commom;
using MicroServices.WebDebts.Domain.Models.Enum;
using MicroServices.WebDebts.Domain.Service;
using MicroServices.WebDebts.Domain.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Application.Service
{
    public interface ICardsApplicationService
    {
        Task<GenericResponse> CreateCard(CardAppModel cardAppModel, Guid userId);
        Task<GenericResponse> AddValuesCard(CreateDebtAppModel createDebtAppModel, Guid cardId, Guid userId);
        Task<GetCardsResponse> GetCardById(Guid id);
        Task<PaginatedList<GetCardsResponse>> FilterCardsAsync(int pageNumber, int pageSize, Guid? id, int? month, int? year, Guid userId, bool withNoDebts);
        Task DeleteCardAsync(Guid cardId);
        Task PayCardDebtsAsync(PayCardResponseModel payCardResponseModel, Guid userId);
        Task EditCardAsync(CardAppModel cardAppModel, Guid userId);
    }
    public class CardsApplicationService : ICardsApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDebtsService _debtsService;
        private readonly IDebtsApplicationService _debtsApplicationService;
        private readonly ICardService _cardService;
        private readonly IUserRepository _userRepository;
        private readonly ICardRepository _cardRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IResponsiblePartyRepository _responsiblePartyRepository;

        public CardsApplicationService(IUnitOfWork unitOfWork, IDebtsService debtsService, ICardService cardService, IUserRepository userRepository, ICardRepository cardRepository, IDebtsApplicationService debtsApplicationService, ICategoryRepository categoryRepository, IResponsiblePartyRepository responsiblePartyRepository)
        {
            _unitOfWork = unitOfWork;
            _cardService = cardService;
            _debtsService = debtsService;
            _userRepository = userRepository;
            _cardRepository = cardRepository;
            _debtsApplicationService = debtsApplicationService;
            _categoryRepository = categoryRepository;
            _responsiblePartyRepository = responsiblePartyRepository;
        }

        public async Task<GenericResponse> AddValuesCard(CreateDebtAppModel debtsAppModel, Guid cardId, Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            var category = await _categoryRepository.FindByIdAsync(debtsAppModel.CategoryId);

            foreach (var debtValue in debtsAppModel.Values)
            {
                var debt = debtsAppModel.ToCreateModel();

                debt.Value = debtValue;
                debt.User = user;
                debt.DebtCategory = category;

                if (debtsAppModel.ResponsiblePartyId.HasValue)
                {
                    var responsibleParty = await _responsiblePartyRepository.FindByIdAsync(debtsAppModel.ResponsiblePartyId.Value);
                    debt.ResponsibleParty = responsibleParty;
                }
                else
                {
                    debt.ResponsibleParty = null;
                }

                var debtCard = await _cardService.LinkCard(debt, cardId);

                var id = Guid.NewGuid();
                await _debtsService.CreateDebtAsync(debtCard, DebtType.Card, id, userId);
                await _unitOfWork.CommitAsync();

                await _unitOfWork.CommitAsync();
            }

            return new GenericResponse { Id = Guid.NewGuid() };
        }

        public async Task<GenericResponse> CreateCard(CardAppModel cardAppModel, Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            var card = cardAppModel.ToEntity();
            card.User = user;

            var cardId = await _cardService.CreateCardAsync(card);
            await _unitOfWork.CommitAsync();

            return new GenericResponse { Id = cardId };
        }

        public async Task<GetCardsResponse> GetCardById(Guid id)
        {
            var card = await _cardService.GetAllByIdAsync(id);
            var cardAppResult = card.ToResponseModel();

            return cardAppResult;
        }

        public async Task<PaginatedList<GetCardsResponse>> FilterCardsAsync(int pageNumber, int pageSize, Guid? id, int? month, int? year, Guid userId, bool withNoDebts)
        {
            var card = await _cardService.FilterCardsAsync(pageNumber, pageSize, id, month, year, userId, withNoDebts);
            var cardAppResult = card.Items.Select(x => x.ToResponseModel()).ToList();
          
            var response = new List<GetCardsResponse>();
            foreach (var cardModel in cardAppResult)
            {
                foreach (var debt in cardModel.Debts.ToList())
                {
                    debt.Installments = debt.Installments.Where(x => x.Date.Month == month.Value && x.Date.Year == year).ToList();
                    debt.Installments = debt.Installments.OrderBy(x => x.InstallmentNumber).ToList();
                }
                response.Add(cardModel);
            }

            return new PaginatedList<GetCardsResponse>
            {
                CurrentPage = pageNumber,
                Items = response,
                TotalItems = card.TotalItems,
                TotalPages = card.TotalPages
            };
        }

        public async Task DeleteCardAsync(Guid cardId)
        {
            var card = await _cardRepository.GetCardById(cardId);

            await _cardRepository.Remove(card);
            await _unitOfWork.CommitAsync();
        }

        public async Task PayCardDebtsAsync(PayCardResponseModel payCardResponseModel, Guid userId)
        {
            var card = await _cardRepository.GetCardById(payCardResponseModel.Id);

            foreach (var debt in card.DebtValues)
            {

                var installment = debt.Installments.FirstOrDefault(x => x.Date.Month == payCardResponseModel.CorrespondingDate.Month && x.Date.Year == payCardResponseModel.CorrespondingDate.Year);

                var putInstallments = new PutInstallmentsRequest
                {
                    Id = installment.Id,
                    InstallmentsStatus = Status.Paid,
                    PaymentDate = payCardResponseModel.PaymentDate,
                };

                await _debtsApplicationService.PutInstallments(putInstallments, userId);
            }
        }

        public async Task EditCardAsync(CardAppModel cardAppModel, Guid id)
        {
            var card = await _cardRepository.GetCardById(id);

            card.ClosureDate = cardAppModel.ClosureDate;
            card.DueDate = cardAppModel.DueDate;
            card.Name = cardAppModel.Name;
            card.Color = cardAppModel.Color;

            foreach (var debt in card.DebtValues)
            {
                foreach (var installment in debt.Installments)
                {
                    if (new DateTime(installment.Date.Year, installment.Date.Month, 1) >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) && installment.Status == Status.NotPaid)
                    {
                        var tempChangeDate = CardRules.CreateClosureAndDueDates(debt.Card.ClosureDate, debt.Card.DueDate, installment.Date);
                        foreach (var dates in tempChangeDate)
                        {
                            if (installment.Date < dates.Key)
                            {
                                installment.Date = dates.Value;
                                break;
                            }
                        }
                    }
                }
            }

            await _unitOfWork.CommitAsync();
        }
    }
}
