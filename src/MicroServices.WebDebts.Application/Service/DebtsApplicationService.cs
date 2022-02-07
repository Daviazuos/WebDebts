using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Models.Mappers;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Commom;
using MicroServices.WebDebts.Domain.Models.Enum;
using MicroServices.WebDebts.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        Task EditDebt(Guid id, CreateDebtAppModel createDebtsRequest, Guid userId);
    }

    public class DebtsApplicationService : IDebtsApplicationService
    {
        private readonly IUserRepository _userRepository;

        private readonly IDebtsService _debtsServices;

        private readonly IDebtRepository _debtRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IWalletRepository _walletRepository;

        private readonly ICardRepository _cardRepository;

        public DebtsApplicationService(IDebtsService debtsServices, IUnitOfWork unitOfWork, IDebtRepository debtRepository, IWalletRepository walletRepository, ICardRepository cardRepository, IUserRepository userRepository)
        {
            _debtsServices = debtsServices;
            _debtRepository = debtRepository;
            _unitOfWork = unitOfWork;
            _walletRepository = walletRepository;
            _cardRepository = cardRepository;
            _userRepository = userRepository;
        }

        public async Task<GenericResponse> CreateDebt(CreateDebtAppModel createDebtsRequest, Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            var debt = createDebtsRequest.ToCreateModel();
            debt.User = user;

            var id = Guid.NewGuid();
            await _debtsServices.CreateDebtAsync(debt, DebtType.Simple, id, userId);
            await _unitOfWork.CommitAsync();
            
            return new GenericResponse { Id = debt.Id };
        }

        public async Task DeletePerson(DeleteDebtByIdRequest deleteDebtByIdRequest)
        {
            await _debtsServices.DeleteDebt(deleteDebtByIdRequest.Id);
            await _unitOfWork.CommitAsync();
        }

        public async Task EditDebt(Guid id, CreateDebtAppModel createDebtsRequest, Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            var oldDebt = await _debtRepository.GetAllByIdAsync(id);
            var debt = createDebtsRequest.ToCreateModel();
            debt.CreatedAt = oldDebt.CreatedAt;
            debt.Card = oldDebt.Card;
            debt.User = user;

            await _debtsServices.DeleteDebt(id);

            await _debtsServices.CreateDebtAsync(debt, DebtType.Simple, id, userId);

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
                                                             userId);


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
                                                                      userId);

            var installmentsApp = debts.Items.Select( x => new FilterInstallmentsResponse { 
                                                                            Date = x.Date, 
                                                                            DebtName = x.Debt.Name,
                                                                            Id = x.Id,
                                                                            InstallmentNumber = x.InstallmentNumber,
                                                                            PaymentDate = x.PaymentDate,
                                                                            Status = (EnumAppModel.StatusApp)x.Status,
                                                                            Value = x.Value});

            return new PaginatedList<FilterInstallmentsResponse>
            {
                Items = installmentsApp.ToList(),
                CurrentPage = debts.CurrentPage,
                TotalItems = debts.TotalItems,
                TotalPages = debts.TotalPages
            };
        }

        public async Task<GetDebtByIdResponse> GetDebtsById(Guid id)
        {
            var debt = await _debtsServices.GetAllByIdAsync(id);

            var debtAppResult = debt.ToResponseModel();

            return debtAppResult;
        }

        public async Task<List<GetSumbyMonthResponse>> GetSumByMonth(GetSumByMonthRequest getSumByMonthRequest, Guid userId)
        {
            var startDate = new DateTime(getSumByMonthRequest.Year.Value, getSumByMonthRequest.Month.Value, 31, 0, 0, 0);
            var finishDate = DateTime.UtcNow.AddMonths(3);

            var installments = await _debtRepository.GetSumPerMonthAsync(getSumByMonthRequest.Month, getSumByMonthRequest.Year, userId);

            var wallets = await _walletRepository.GetWalletByMonth(getSumByMonthRequest.Month, getSumByMonthRequest.Year, userId);

            var groupedWallet = wallets.GroupBy(x => new { x.StartAt.Month, x.StartAt.Year, x.FinishAt })
                    .Select(g => new
                    {
                        WalletValue = g.Sum(s => s.Value),
                        WalletStartDate = g.First().StartAt,
                        WalletFinishDate = g.First().FinishAt

                    }).ToList();

            var walletsPerDate = new Dictionary<string, List<Decimal>>();

            for (var dt = startDate; dt <= finishDate; dt = dt.AddMonths(1))
            {
                foreach(var wallet in groupedWallet)
                {
                    if (dt > wallet.WalletStartDate)
                    {
                        if (wallet.WalletFinishDate.HasValue)
                        {
                            var dayOne = new DateTime(dt.Year, dt.Month, 1);
                            if (dayOne <= wallet.WalletFinishDate.Value)
                            {
                                if (walletsPerDate.GetValueOrDefault(dt.Month.ToString() + dt.Year.ToString(), null) == null)
                                {
                                    var listValues = new List<decimal>();
                                    listValues.Add(wallet.WalletValue);
                                    walletsPerDate.Add(dt.Month.ToString() + dt.Year.ToString(), listValues);
                                }
                                else
                                {
                                    walletsPerDate[dt.Month.ToString() + dt.Year.ToString()].Add(wallet.WalletValue);
                                }
                                
                            }
                        }
                        else
                        {
                            if (walletsPerDate.GetValueOrDefault(dt.Month.ToString() + dt.Year.ToString(), null) == null)
                            {
                                var listValues = new List<decimal>();
                                listValues.Add(wallet.WalletValue);
                                walletsPerDate.Add(dt.Month.ToString() + dt.Year.ToString(), listValues);
                            }
                            else
                            {
                                walletsPerDate[dt.Month.ToString() + dt.Year.ToString()].Add(wallet.WalletValue);
                            }

                        }
                    }
                }
            }

            var intToMonth = new Dictionary<int, string>();
            intToMonth.Add(1, "Janeiro");
            intToMonth.Add(2, "Fevereiro");
            intToMonth.Add(3, "Março");
            intToMonth.Add(4, "Abril");
            intToMonth.Add(5, "Maio");
            intToMonth.Add(6, "Junho");
            intToMonth.Add(7, "Julho");
            intToMonth.Add(8, "Agosto");
            intToMonth.Add(9, "Setembro");
            intToMonth.Add(10, "Outubro");
            intToMonth.Add(11, "Novembro");
            intToMonth.Add(12, "Dezembro");

            var defaultValue = new List<decimal>();
            defaultValue.Add(new decimal(0.00));

            var sumValues = installments.GroupBy(d => new { d.Date.Month, d.Date.Year })
                                    .Select(
                                        g => new GetSumbyMonthResponse
                                        {
                                            DebtValue = g.Sum(s => s.Value),
                                            WalletValue = walletsPerDate.GetValueOrDefault(g.First().Date.Month.ToString() + g.First().Date.Year.ToString(), defaultValue).Sum(x => x),
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
                    var installments = await _debtRepository.FilterInstallmentsAsync(1, 9999, null, putInstallmentsRequest.CardId.Value, putInstallmentsRequest.PaymentDate.Value.Month, putInstallmentsRequest.PaymentDate.Value.Year, null, null, null, userId);
                    
                    var walletCardId = await _walletRepository.SubtractWalletMonthControllers(putInstallmentsRequest.WalletId.Value,
                                                                                      putInstallmentsRequest.PaymentDate.Value.Month,
                                                                                      putInstallmentsRequest.PaymentDate.Value.Year,
                                                                                      installments.Items.Sum(x => x.Value));

                    foreach (var debt in installments.Items)
                    {
                        await _debtRepository.UpdateInstallmentAsync(debt.Id, putInstallmentsRequest.InstallmentsStatus, putInstallmentsRequest.PaymentDate, walletCardId);
                    }

                }
                else
                {
                    var installment = await _debtRepository.GetInstallmentById(putInstallmentsRequest.Id.Value);
                    var walletId = await _walletRepository.SubtractWalletMonthControllers(putInstallmentsRequest.WalletId.Value,
                                                                                          putInstallmentsRequest.PaymentDate.Value.Month,
                                                                                          putInstallmentsRequest.PaymentDate.Value.Year,
                                                                                          installment.Value);

                    await _debtRepository.UpdateInstallmentAsync(putInstallmentsRequest.Id.Value, putInstallmentsRequest.InstallmentsStatus, putInstallmentsRequest.PaymentDate, walletId);
                }
                
            }
            else
            {
                if (putInstallmentsRequest.CardId.HasValue)
                {
                    var installments = await _debtRepository.FilterInstallmentsAsync(1, 9999, null, cardId: putInstallmentsRequest.CardId.Value, putInstallmentsRequest.PaymentDate.Value.Month, putInstallmentsRequest.PaymentDate.Value.Year, null, null, DebtType.Card, userId);

                    var monthControllerId = installments.Items.Select(x => x.WalletMonthControllerId.Value).FirstOrDefault();
                    
                    await _walletRepository.AddWalletMonthControllers(monthControllerId, installments.Items.Sum(x => x.Value));

                    foreach (var debt in installments.Items)
                    {
                        await _debtRepository.UpdateInstallmentAsync(debt.Id, putInstallmentsRequest.InstallmentsStatus, putInstallmentsRequest.PaymentDate, null);
                    }
                }
                else
                {
                    var installment = await _debtRepository.GetInstallmentById(putInstallmentsRequest.Id.Value);
                    await _walletRepository.AddWalletMonthControllers(installment.WalletMonthControllerId.Value, installment.Value);
                    await _debtRepository.UpdateInstallmentAsync(putInstallmentsRequest.Id.Value, putInstallmentsRequest.InstallmentsStatus, putInstallmentsRequest.PaymentDate, null);
                }
            }
            
            
            await _unitOfWork.CommitAsync();
        }
    }
}
