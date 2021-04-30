using MicroServices.WebDebts.Domain.Common;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static MicroServices.WebDebts.Domain.Service.InstallmentsStrategy;

namespace MicroServices.WebDebts.Domain.Services
{
    public interface IDebtsService
    {
        Task CreateDebtAsync(Debt debt, DebtType debtType);
        Task<Debt> GetAllByIdAsync(Guid id);

        Task<List<Debt>> FilterDebtsAsync(string name, 
                                         decimal? value, 
                                         DateTime? startDate, 
                                         DateTime? finishDate, 
                                         DebtInstallmentType? debtInstallmentType, 
                                         DebtType? debtType);
        Task DeleteDebt(Guid id);
    }

    public class DebtsService : IDebtsService
    {
        private readonly IDebtRepository _debtRepository;

        public DebtsService(IDebtRepository debtRepository)
        {
            _debtRepository = debtRepository;
        }

        public async Task CreateDebtAsync(Debt debt, DebtType debtType)
        {
            var classInstallments = new InstallmentsContext();

            if (debtType == DebtType.Card)
            {
                debt.Date = CardRules.CardDateRule(debt.Card.ClosureDate, debt.Card.DueDate, debt).Date;
            }

            if (debt.DebtInstallmentType == DebtInstallmentType.Simple)
                classInstallments.SetStrategy(new CreateSimpleInstallments());
            else if (debt.DebtInstallmentType == DebtInstallmentType.Fixed)
                classInstallments.SetStrategy(new CreateFixedInstallments());
            else if (debt.DebtInstallmentType == DebtInstallmentType.Installment)
            {
                classInstallments.SetStrategy(new CreateInstallments());
                debt.Value = debt.Value * debt.NumberOfInstallments;
            }

            var installments = classInstallments.CreateInstallments(debt);

            debt.Installments = installments;
            debt.Id = Guid.NewGuid();
            debt.CreatedAt = DateTime.Now;
            debt.DebtType = debtType;

            await _debtRepository.AddAsync(debt);
        }

        public async Task DeleteDebt(Guid id)
        {
            await _debtRepository.DeleteDebt(id);
        }

        public async Task<List<Debt>> FilterDebtsAsync(string name, decimal? value, DateTime? startDate, DateTime? finishDate, DebtInstallmentType? debtInstallmentType, DebtType? debtType)
        {
            return await _debtRepository.FindDebtAsync(name, value, startDate, finishDate, debtInstallmentType, debtType);
        }

        public async Task<Debt> GetAllByIdAsync(Guid id)
        {
            return await _debtRepository.GetAllByIdAsync(id);
        }
    }
}
