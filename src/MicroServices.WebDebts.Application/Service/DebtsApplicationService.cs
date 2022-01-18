using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Models.Mappers;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
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
        Task<GenericResponse> CreateDebt(CreateDebtAppModel createDebtsRequest);
        Task<GetDebtByIdResponse> GetDebtsById(Guid id);
        Task DeletePerson(DeleteDebtByIdRequest deletePersonRequest);
        Task<PaginatedList<GetDebtByIdResponse>> FilterDebtsById(FilterDebtRequest filterDebtRequest);
        Task<PaginatedList<FilterInstallmentsResponse>> FilterInstallments(FilterInstallmentsRequest filterInstallmentsRequest);
        Task PutInstallments(PutInstallmentsRequest putInstallmentsRequest);
        Task<List<GetSumbyMonthResponse>> GetSumByMonth(GetSumByMonthRequest getSumByMonthRequest);
    }

    public class DebtsApplicationService : IDebtsApplicationService
    {
        private readonly IDebtsService _debtsServices;

        private readonly IDebtRepository _debtRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IWalletRepository _walletRepository;

        private readonly ICardRepository _cardRepository;

        public DebtsApplicationService(IDebtsService debtsServices, IUnitOfWork unitOfWork, IDebtRepository debtRepository, IWalletRepository walletRepository, ICardRepository cardRepository)
        {
            _debtsServices = debtsServices;
            _debtRepository = debtRepository;
            _unitOfWork = unitOfWork;
            _walletRepository = walletRepository;
            _cardRepository = cardRepository;
        }

        public async Task<GenericResponse> CreateDebt(CreateDebtAppModel createDebtsRequest)
        {
            var debt = createDebtsRequest.ToCreateModel();
            
            await _debtsServices.CreateDebtAsync(debt, DebtType.Simple);
            await _unitOfWork.CommitAsync();
            
            return new GenericResponse { Id = debt.Id };
        }

        public async Task DeletePerson(DeleteDebtByIdRequest deleteDebtByIdRequest)
        {
            await _debtsServices.DeleteDebt(deleteDebtByIdRequest.Id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<PaginatedList<GetDebtByIdResponse>> FilterDebtsById(FilterDebtRequest filterDebtRequest)
        {
            var debt = await _debtsServices.FilterDebtsAsync(filterDebtRequest.PageNumber,
                                                             filterDebtRequest.Name, 
                                                             filterDebtRequest.Value, 
                                                             filterDebtRequest.StartDate,
                                                             filterDebtRequest.FInishDate,
                                                             (DebtInstallmentType?)filterDebtRequest.DebtInstallmentType, 
                                                             (DebtType?)filterDebtRequest.DebtType);


            var debtAppResult = debt.Items.Select(x => x.ToResponseModel()).ToList();

            return new PaginatedList<GetDebtByIdResponse>
            {
                CurrentPage = debt.CurrentPage,
                Items = debtAppResult,
                TotalItems = debt.TotalItems,
                TotalPages = debt.TotalPages
            };
        }

        public async Task<PaginatedList<FilterInstallmentsResponse>> FilterInstallments(FilterInstallmentsRequest filterInstallmentsRequest)
        {
            var debts = await _debtRepository.FilterInstallmentsAsync(filterInstallmentsRequest.PageNumber,
                                                                      filterInstallmentsRequest.DebtId, 
                                                                      filterInstallmentsRequest.Month, 
                                                                      filterInstallmentsRequest.Year, 
                                                                      (DebtInstallmentType?)filterInstallmentsRequest.DebtInstallmentType,
                                                                      (Status?)filterInstallmentsRequest.StatusApp);

            var installmentsApp = debts.Items.Select( x => new FilterInstallmentsResponse { 
                                                                            Date = x.Date, 
                                                                            DebtName = x.Debt.Name,
                                                                            Id = x.Id,
                                                                            InstallmentNumber = x.InstallmentNumber,
                                                                            PaymentDate = x.PaymentDate,
                                                                            Status = (EnumAppModel.StatusApp)x.Status,
                                                                            Value = x.Value}).OrderBy(x => x.Date);

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

        public async Task<List<GetSumbyMonthResponse>> GetSumByMonth(GetSumByMonthRequest getSumByMonthRequest)
        {
            var startDate = new DateTime(getSumByMonthRequest.Year.Value, getSumByMonthRequest.Month.Value, 31, 0, 0, 0);
            var finishDate = DateTime.UtcNow.AddMonths(1);

            var installments = await _debtRepository.GetSumPerMonthAsync(getSumByMonthRequest.Month, getSumByMonthRequest.Year);

            var wallets = await _walletRepository.GetWalletByMonth(getSumByMonthRequest.Month, getSumByMonthRequest.Year);

            var groupedWallet = wallets.GroupBy(x => new { x.StartAt.Month, x.StartAt.Year })
                    .Select(g => new
                    {
                        WalletValue = g.Sum(s => s.Value),
                        WalletStartDate = g.First().StartAt,
                        WalletFinishDate = g.First().FinishAt

                    }).ToList();

            var walletsPerDate = new Dictionary<string, decimal>();

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
                                walletsPerDate.Add(dt.Month.ToString() + dt.Year.ToString(), wallet.WalletValue);
                                break;
                            }
                        }
                        else
                        {
                            walletsPerDate.Add(dt.Month.ToString() + dt.Year.ToString(), wallet.WalletValue);
                            break;
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

            var defaultValue = new decimal(0.00);

            var sumValues = installments.GroupBy(d => new { d.Date.Month, d.Date.Year })
                                    .Select(
                                        g => new GetSumbyMonthResponse
                                        {
                                            DebtValue = g.Sum(s => s.Value),
                                            WalletValue = walletsPerDate.GetValueOrDefault(g.First().Date.Month.ToString() + g.First().Date.Year.ToString(), defaultValue),
                                            Month = intToMonth[g.First().Date.Month],
                                            Year = g.First().Date.Year,
                                            OriginalDate = g.First().Date
                                        }).ToList();

            sumValues = sumValues.OrderBy(x => x.OriginalDate).ToList();

            return sumValues;
        }

        public async Task PutInstallments(PutInstallmentsRequest putInstallmentsRequest)
        {
            if (putInstallmentsRequest.InstallmentsStatus == Status.Paid)
            {
                var installment = await _debtRepository.GetInstallmentById(putInstallmentsRequest.Id);
                var walletId = await _walletRepository.SubtractWalletMonthControllers(putInstallmentsRequest.WalletId.Value,
                                                                       putInstallmentsRequest.PaymentDate.Value.Month,
                                                                       putInstallmentsRequest.PaymentDate.Value.Year,
                                                                       installment.Value);
                await _debtRepository.UpdateInstallmentAsync(putInstallmentsRequest.Id, putInstallmentsRequest.InstallmentsStatus, putInstallmentsRequest.PaymentDate, walletId);
            }
            else
            {
                var installment = await _debtRepository.GetInstallmentById(putInstallmentsRequest.Id);
                await _walletRepository.AddWalletMonthControllers(installment.WalletMonthControllerId.Value,
                                                                  installment.Value);
                await _debtRepository.UpdateInstallmentAsync(putInstallmentsRequest.Id, putInstallmentsRequest.InstallmentsStatus, putInstallmentsRequest.PaymentDate, null);
            }
            
            
            await _unitOfWork.CommitAsync();
        }
    }
}
