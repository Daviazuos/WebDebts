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
        Task<PaginatedList<Debt>> FindDebtAsync(int pageNumber, string name, decimal? value, DateTime? startDate, DateTime? finishDate, DebtInstallmentType? debtInstallmentType, DebtType? debtType, Guid userId);
        Task<PaginatedList<Installments>> FilterInstallmentsAsync(int pageNumber, int pageSize, Guid? debtId, int? month, int? year, DebtInstallmentType? debtInstallmentType, Status? status, DebtType? debtType, Guid userId);
        Task UpdateInstallmentAsync(Guid id, Status status, DateTime? paymentDate, Guid? walletId);
        Task<List<Installments>> GetSumPerMonthAsync(int? month, int? year, Guid userId);
        Task<Installments> GetInstallmentById(Guid installmentId);
    }
}
