using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Commom;
using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface IDebtRepository : IBaseRepository<Debt>
    {
        Task<Debt> GetAllByIdAsync(Guid Id);
        Task DeleteDebt(Guid Id);
        Task<PaginatedList<Debt>> FindDebtAsync(int pageNumber, string name, decimal? value, DateTime? startDate, DateTime? finishDate, DebtInstallmentType? debtInstallmentType, DebtType? debtType);
        Task<PaginatedList<Installments>> FilterInstallmentsAsync(int pageNumber, Guid? debtId, int? month, int? year, DebtInstallmentType? debtInstallmentType, Status? status);
        Task UpdateInstallmentAsync(Guid id, Status status, DateTime? paymentDate, Guid? walletId);
        Task<List<Installments>> GetSumPerMonthAsync(int? month, int? year);
        Task<Installments> GetInstallmentById(Guid installmentId);
    }
}
