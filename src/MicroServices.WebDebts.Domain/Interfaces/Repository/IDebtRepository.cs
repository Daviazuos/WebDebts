using MicroServices.WebDebts.Domain.Models;
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
        Task<List<Debt>> FindDebtAsync(string name, decimal? value, DateTime? startDate, DateTime? finishDate, DebtInstallmentType? debtInstallmentType, DebtType? debtType);
        Task<List<Installments>> FilterInstallmentsAsync(Guid? debtId, int? month, int? year, DebtInstallmentType? debtInstallmentType, Status? status);
        Task UpdateInstallmentAsync(Guid id, Status status);
        Task<List<Installments>> GetSumPerMonthAsync(int? month, int? year);
    }
}
