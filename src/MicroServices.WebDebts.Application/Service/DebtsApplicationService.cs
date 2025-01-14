using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Models.Mappers;
using MicroServices.WebDebts.Application.Models.WalletModels;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Commom;
using MicroServices.WebDebts.Domain.Models.Enum;
using MicroServices.WebDebts.Domain.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using static MicroServices.WebDebts.Application.Models.EnumAppModel;

namespace MicroServices.WebDebts.Application.Services
{
    public interface IDebtsApplicationService
    {
        Task<GenericResponse> CreateDebt(CreateDebtAppModel createDebtsRequest, Guid userId);
        Task<GetDebtByIdResponse> GetDebtsById(Guid id);
        Task DeletePerson(DeleteDebtByIdRequest deletePersonRequest);
        Task<PaginatedList<GetDebtByIdResponse>> FilterDebtsById(FilterDebtRequest filterDebtRequest, Guid userId);
        Task<PaginatedList<FilterInstallmentsResponse>> FilterInstallments(FilterInstallmentsRequest filterInstallmentsRequest, Guid userId);
        Task PutInstallments(PutInstallmentsRequest putInstallmentsRequest, Guid userId);
        Task<List<GetSumbyMonthResponse>> GetSumByMonth(GetSumByMonthRequest getSumByMonthRequest, Guid userId);
        Task EditDebt(Guid id, UpdateDebtAppModel createDebtsRequest, Guid userId);
        Task<List<GetCategoryRequest>> GetCategories(Guid userId);
        Task<GenericResponse> CreateCategory(CreateCategoryRequest createCategoryRequest, Guid userId);
        Task<List<GetDebtCategoryResponse>> GetDebtCategories(FilterDebtsCategoriesRequest filterDebtsCategoriesRequest, Guid userId);
        Task<GetAnaliticsResponse> GetAnaliticsByMonth(GetAnaliticsRequest getAnaliticsRequest, Guid userId);
        Task EditInstallments(Guid id, InstallmentsAppModel installmentsAppModel, Guid userId);
        Task DeleteInstallment(Guid id);
        Task<List<GetDebtResponsiblePartiesResponse>> GetResponsiblePartiesDebts(Guid? responsiblePartyId, int month, int year);
    }

    public class DebtsApplicationService : IDebtsApplicationService
    {
        private readonly IUserRepository _userRepository;

        private readonly IDebtsService _debtsServices;

        private readonly IDebtRepository _debtRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IWalletRepository _walletRepository;

        private readonly ICardRepository _cardRepository;

        private readonly ICategoryRepository _categoryRepository;

        private readonly IResponsiblePartyRepository _responsiblePartyRepository;

        public DebtsApplicationService(IDebtsService debtsServices, IUnitOfWork unitOfWork, IDebtRepository debtRepository, IWalletRepository walletRepository, ICardRepository cardRepository, IUserRepository userRepository, ICategoryRepository categoryRepository, IResponsiblePartyRepository responsiblePartyRepository)
        {
            _debtsServices = debtsServices;
            _debtRepository = debtRepository;
            _unitOfWork = unitOfWork;
            _walletRepository = walletRepository;
            _cardRepository = cardRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _responsiblePartyRepository = responsiblePartyRepository;
        }

        public async Task<GenericResponse> CreateCategory(CreateCategoryRequest createCategoryRequest, Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            var id = Guid.NewGuid();

            var category = createCategoryRequest.ToEntity();
            category.User = user;
            category.Id = id;

            await _debtsServices.CreateCategoryAsync(category);
            await _unitOfWork.CommitAsync();

            return new GenericResponse { Id = category.Id };
        }

        public async Task<GenericResponse> CreateDebt(CreateDebtAppModel createDebtsRequest, Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            var category = await _categoryRepository.FindByIdAsync(createDebtsRequest.CategoryId);
            
            foreach (var debtValue in createDebtsRequest.Values)
            {
                var debt = createDebtsRequest.ToCreateModel();
                debt.User = user;
                debt.DebtCategory = category;
                debt.Value = debtValue;

                if (createDebtsRequest.ResponsiblePartyId.HasValue)
                {
                    var responsibleParty = await _responsiblePartyRepository.FindByIdAsync(createDebtsRequest.ResponsiblePartyId.Value);
                    debt.ResponsibleParty = responsibleParty;
                }
                else
                {
                    debt.ResponsibleParty = null;
                }

                var id = Guid.NewGuid();
                await _debtsServices.CreateDebtAsync(debt, DebtType.Simple, id, userId);
                await _unitOfWork.CommitAsync();
            }

            return new GenericResponse { Id = Guid.NewGuid() };
        }

        public async Task DeleteInstallment(Guid id)
        {
            await _debtRepository.DeleteInstallment(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeletePerson(DeleteDebtByIdRequest deleteDebtByIdRequest)
        {
            var debt = await _debtRepository.GetAllByIdAsync(deleteDebtByIdRequest.Id);
            debt.Status = DebtStatus.Closed;

            foreach (var installment in debt.Installments.ToList())
            {
                if (installment.Status != Status.Paid)
                {
                    await _debtRepository.DeleteInstallment(installment.Id);
                }
            }

            await _unitOfWork.CommitAsync();

            if (debt.Installments.Count == 0)
            {
                await _debtRepository.DeleteDebt(debt.Id);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task EditDebt(Guid id, UpdateDebtAppModel createDebtsRequest, Guid userId)
        {
            var category = await _categoryRepository.FindByIdAsync(createDebtsRequest.CategoryId);

            var debt = await _debtRepository.GetAllByIdAsync(id);
            debt.DebtCategory = category;
            debt.Name = createDebtsRequest.Name;

            foreach (var installment in debt.Installments)
            {
                var nowDate = DateTime.UtcNow.Date;
                if (new DateTime(installment.Date.Year, installment.Date.Month, 1) >= new DateTime(nowDate.Date.Year, nowDate.Date.Month, 1))
                {
                    installment.Value = createDebtsRequest.Value;
                }
            }
            debt.Value = debt.Installments.Sum(x => x.Value);

            await _unitOfWork.CommitAsync();
        }

        public async Task EditInstallments(Guid id, InstallmentsAppModel installmentsAppModel, Guid userId)
        {
            var installment = await _debtRepository.GetInstallmentById(id);

            installment.Date = installmentsAppModel.Date;
            installment.PaymentDate = installmentsAppModel.PaymentDate;
            installment.InstallmentNumber = installmentsAppModel.InstallmentNumber;
            installment.Value = installmentsAppModel.Value;

            await _debtRepository.EditInstallmentAsync(installment);
            await _unitOfWork.CommitAsync();
        }

        public async Task<PaginatedList<GetDebtByIdResponse>> FilterDebtsById(FilterDebtRequest filterDebtRequest, Guid userId)
        {
            var debt = await _debtsServices.FilterDebtsAsync(filterDebtRequest.PageNumber,
                                                             filterDebtRequest.PageSize,
                                                             filterDebtRequest.Name,
                                                             filterDebtRequest.Value,
                                                             filterDebtRequest.StartDate,
                                                             filterDebtRequest.FInishDate,
                                                             (DebtInstallmentType?)filterDebtRequest.DebtInstallmentType,
                                                             (DebtType?)filterDebtRequest.DebtType,
                                                             filterDebtRequest.Category,
                                                             userId,
                                                             filterDebtRequest.IsGoal);


            var debtAppResult = debt.Items.Select(x => x.ToResponseModel()).ToList();

            foreach (var debtResult in debtAppResult)
            {
                var maxPayment = debtResult.Installments.Max(x => x.PaymentDate);
                if (maxPayment.HasValue)
                {
                    var paidInstallment = debtResult.Installments.FirstOrDefault(x => x.PaymentDate == maxPayment).InstallmentNumber;
                    debtResult.PaidInstallment = paidInstallment;
                }
            }

            return new PaginatedList<GetDebtByIdResponse>
            {
                CurrentPage = debt.CurrentPage,
                Items = debtAppResult,
                TotalItems = debt.TotalItems,
                TotalPages = debt.TotalPages
            };
        }

        public async Task<PaginatedList<FilterInstallmentsResponse>> FilterInstallments(FilterInstallmentsRequest filterInstallmentsRequest, Guid userId)
        {
            var debts = await _debtRepository.FilterInstallmentsAsync(filterInstallmentsRequest.PageNumber,
                                                                      filterInstallmentsRequest.PageSize,
                                                                      filterInstallmentsRequest.DebtId,
                                                                      filterInstallmentsRequest.CardId,
                                                                      filterInstallmentsRequest.Month,
                                                                      filterInstallmentsRequest.Year,
                                                                      (DebtInstallmentType?)filterInstallmentsRequest.DebtInstallmentType,
                                                                      (Status?)filterInstallmentsRequest.StatusApp,
                                                                      (DebtType?)filterInstallmentsRequest.DebtType,
                                                                      userId,
                                                                      filterInstallmentsRequest.IsGoal);

            var installmentsApp = debts.Items.Select(x => new FilterInstallmentsResponse
            {
                Date = (x.BuyDate == new DateTime()) ? x.Date : x.BuyDate,
                DebtName = x.Debt?.Name,
                Id = x.Id,
                InstallmentNumber = x.InstallmentNumber,
                NumberOfInstallments = x.Debt.NumberOfInstallments,
                PaymentDate = x.PaymentDate,
                Status = (EnumAppModel.StatusApp)x.Status,
                Value = x.Value,
                Category = x.Debt?.DebtCategory?.Name
            });

            return new PaginatedList<FilterInstallmentsResponse>
            {
                Items = installmentsApp.ToList(),
                CurrentPage = debts.CurrentPage,
                TotalItems = debts.TotalItems,
                TotalPages = debts.TotalPages
            };
        }

        public async Task<GetAnaliticsResponse> GetAnaliticsByMonth(GetAnaliticsRequest getAnaliticsRequest, Guid userId)
        {
            var wallet = await _walletRepository.GetWallets(new WalletStatus(), getAnaliticsRequest.Month, getAnaliticsRequest.Year, userId);
            var installments = await _debtRepository.GetSumPerMonthAsync(getAnaliticsRequest.Month, getAnaliticsRequest.Year, userId);
            var cards = await _cardRepository.FindCardValuesByIdAsync(1, 9999, null, userId, getAnaliticsRequest.Month, getAnaliticsRequest.Year, true);

            var sumWallet = wallet.Sum(x => x.Value);
            var sumInstallments = installments.Sum(x => x.Value);

            var valueDiff = sumWallet - sumInstallments;

            return new GetAnaliticsResponse();

        }

        public async Task<List<GetCategoryRequest>> GetCategories(Guid userId)
        {
            var response = await _categoryRepository.GetCategories(userId);
            var categories = response.Select(x => new GetCategoryRequest { Id = x.Id, Name = x.Name }).ToList();
            return categories;
        }

        public class InstallmentDto
        {
            public string DebtName { get; set; }
            public Guid Id { get; set; }
            public string Category { get; set; }
            public Decimal Value { get; set; }

        }

        public async Task<List<GetDebtCategoryResponse>> GetDebtCategories(FilterDebtsCategoriesRequest filterDebtsCategoriesRequest, Guid userId)
        {
            var installments = await _debtRepository.FilterInstallmentsAsync(1, 9999, null, filterDebtsCategoriesRequest.CardId, filterDebtsCategoriesRequest.Month, filterDebtsCategoriesRequest.Year, null, null, null, userId, null);

            var valueTotal = installments.Items.Sum(x => x.Value);

            var groupedCategories = installments.Items.GroupBy(x => new { x.Debt?.DebtCategory?.Name })
                    .Select(g => new GetDebtCategoryResponse
                    {
                        Month = filterDebtsCategoriesRequest.Month,
                        Name = g.First().Debt.DebtCategory?.Name,
                        Value = g.Sum(s => s.Value),
                        ValueTotal = valueTotal,
                        Percent = Math.Round(g.Sum(s => s.Value) / valueTotal * 100, 2),
                        InstallmentsPerCategory = g.Select(installment => new InstallmentDto
                        {
                            DebtName = installment.Debt.Name,
                            Id = installment.Id,
                            Category = g.First().Debt.DebtCategory?.Name,
                            Value = installment.Value,
                        }).ToList()
                    }).ToList();

            return groupedCategories;

        }

        public async Task<GetDebtByIdResponse> GetDebtsById(Guid id)
        {
            var debt = await _debtsServices.GetAllByIdAsync(id);

            var debtAppResult = debt.ToResponseModel();
            var maxPayment = debtAppResult.Installments.Max(x => x.PaymentDate);
            if (maxPayment.HasValue)
            {
                var paidInstallment = debtAppResult.Installments.FirstOrDefault(x => x.PaymentDate == maxPayment).InstallmentNumber;
                debtAppResult.PaidInstallment = paidInstallment;
            }

            return debtAppResult;
        }

        public async Task<List<GetSumbyMonthResponse>> GetSumByMonth(GetSumByMonthRequest getSumByMonthRequest, Guid userId)
        {
            var startDate = new DateTime(getSumByMonthRequest.Year.Value,
                                         getSumByMonthRequest.Month.Value,
                                         DateTime.DaysInMonth(getSumByMonthRequest.Year.Value, getSumByMonthRequest.Month.Value), 0, 0, 0);

            var finishDate = startDate.AddMonths(4);

            var installments = await _debtRepository.GetSumPerMonthAsync(getSumByMonthRequest.Month, getSumByMonthRequest.Year, userId);

            var walletsInstallments = await _walletRepository.GetWalletInstallmentByMonth(getSumByMonthRequest.Month, getSumByMonthRequest.Year, userId);

            var groupedWallets = new Dictionary<string, List<Decimal>>();

            foreach (var walletInstallment in walletsInstallments)
            {
                var key = walletInstallment.Date.Month.ToString() + walletInstallment.Date.Year.ToString();
                if (groupedWallets.ContainsKey(key))
                {
                    groupedWallets[walletInstallment.Date.Month.ToString() + walletInstallment.Date.Year.ToString()].Add(walletInstallment.Value);
                }
                else
                {
                    groupedWallets.Add(key, new List<Decimal>() { walletInstallment.Value });
                }

            }


            var intToMonth = new Dictionary<int, string>
            {
                { 1, "Janeiro" },
                { 2, "Fevereiro" },
                { 3, "Março" },
                { 4, "Abril" },
                { 5, "Maio" },
                { 6, "Junho" },
                { 7, "Julho" },
                { 8, "Agosto" },
                { 9, "Setembro" },
                { 10, "Outubro" },
                { 11, "Novembro" },
                { 12, "Dezembro" }
            };

            var defaultValue = new List<decimal>();

            var sumValues = installments.GroupBy(d => new { d.Date.Month, d.Date.Year })
                                    .Select(
                                        g => new GetSumbyMonthResponse
                                        {
                                            DebtValue = g.Sum(s => s.Value),
                                            WalletValue = groupedWallets.GetValueOrDefault(g.First().Date.Month.ToString() + g.First().Date.Year.ToString(), defaultValue).Sum(x => x),
                                            Month = intToMonth[g.First().Date.Month],
                                            Year = g.First().Date.Year,
                                            OriginalDate = g.First().Date
                                        }).ToList();

            sumValues = sumValues.OrderBy(x => x.OriginalDate).ToList();

            return sumValues;
        }

        public async Task PutInstallments(PutInstallmentsRequest putInstallmentsRequest, Guid userId)
        {
            if (putInstallmentsRequest.InstallmentsStatus == Status.Paid)
            {
                if (putInstallmentsRequest.CardId.HasValue)
                {
                    var installments = await _debtRepository.FilterInstallmentsAsync(1, 9998, null, putInstallmentsRequest.CardId.Value, putInstallmentsRequest.PaymentDate.Value.Month, putInstallmentsRequest.PaymentDate.Value.Year, null, null, null, userId, null);

                    foreach (var debt in installments.Items)
                    {
                        await _debtRepository.UpdateInstallmentAsync(debt.Id, putInstallmentsRequest.InstallmentsStatus, putInstallmentsRequest.PaymentDate);
                    }
                }
                else
                {
                    await _debtRepository.UpdateInstallmentAsync(putInstallmentsRequest.Id.Value, putInstallmentsRequest.InstallmentsStatus, putInstallmentsRequest.PaymentDate);
                }

            }
            else
            {
                if (putInstallmentsRequest.CardId.HasValue)
                {
                    var installments = await _debtRepository.FilterInstallmentsAsync(1, 9999, null, cardId: putInstallmentsRequest.CardId.Value, putInstallmentsRequest.PaymentDate.Value.Month, putInstallmentsRequest.PaymentDate.Value.Year, null, null, DebtType.Card, userId, null);

                    foreach (var debt in installments.Items)
                    {
                        await _debtRepository.UpdateInstallmentAsync(debt.Id, putInstallmentsRequest.InstallmentsStatus, putInstallmentsRequest.PaymentDate);
                    }
                }
                else
                {
                    await _debtRepository.UpdateInstallmentAsync(putInstallmentsRequest.Id.Value, putInstallmentsRequest.InstallmentsStatus, putInstallmentsRequest.PaymentDate);
                }
            }


            await _unitOfWork.CommitAsync();
        }

        public async Task<List<GetDebtResponsiblePartiesResponse>> GetResponsiblePartiesDebts(Guid? responsiblePartyId, int month, int year)
        {
            var debtsresponsibleParties = await _debtRepository.GetDebtResposibleParty(responsiblePartyId, month, year);
            var walletResponsibleParties = await _walletRepository.GetWalletResposibleParty(responsiblePartyId, month, year);

            var response = debtsresponsibleParties
                .GroupBy(d => d.ResponsibleParty.Id)
                .Select(g =>
                {
                    var firstDebt = g.FirstOrDefault(); // Verifica se há elementos
                    if (firstDebt == null)
                        return null;

                    var walletGroup = walletResponsibleParties.GroupBy(d => d.ResponsibleParty);

                    // Soma todos os valores de dívida no grupo
                    var debtValue = g.Sum(s => s.Installments.FirstOrDefault()?.Value ?? 0);

                    // Soma todos os valores correspondentes no grupo de carteira
                    var walletValue = walletGroup
                        .Where(x => x.Key.Id == firstDebt.ResponsibleParty.Id)
                        .SelectMany(x => x)
                        .Sum(x => x.WalletInstallments.Sum(x => x.Value)); // Somar todos os valores

                    // Lista todos os modelos de carteira
                    var walletAppModels = walletGroup
                        .Where(x => x.Key.Id == firstDebt.ResponsibleParty.Id)
                        .SelectMany(x => x)
                        .Select(x => x.ToAppModel())
                        .ToList();

                    return new GetDebtResponsiblePartiesResponse
                    {
                        Name = firstDebt.ResponsibleParty.Name,
                        DebtValue = debtValue,
                        WalletValue = walletValue,
                        DebtsAppModel = g.Select(x => x.ToModel()).ToList(),
                        WalletAppModels = walletAppModels,
                    };
                })
                .Where(response => response != null) // Remove itens nulos gerados pela validação
                .ToList();

            return response;
        }


    }
}
