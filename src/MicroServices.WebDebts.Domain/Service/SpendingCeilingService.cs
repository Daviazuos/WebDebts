using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Service
{
    public interface ISpendingCeilingService
    {
        Task<Guid> CreateSpendingCeiling(SpendingCeiling spendingCeiling);
        Task<List<SpendingCeiling>> GetSpendingCeilingByMonth(Guid userId, int month, int year);
    }
    public class SpendingCeilingService : ISpendingCeilingService
    {
        private readonly ISpendingCeilingRepository _spendingCeilingRepository;
        public SpendingCeilingService(ISpendingCeilingRepository spendingCeilingRepository)
        {
            _spendingCeilingRepository = spendingCeilingRepository;
        }

        public async Task<Guid> CreateSpendingCeiling(SpendingCeiling spendingCeiling)
        {
            spendingCeiling.CreatedAt = DateTime.Now;

            await _spendingCeilingRepository.AddAsync(spendingCeiling);

            return spendingCeiling.Id;
        }

        public async Task<List<SpendingCeiling>> GetSpendingCeilingByMonth(Guid userId, int month, int year)
        {
            var spendingCeilings = await _spendingCeilingRepository.GetSpendingCeilingByMonth(month, year, userId);

            return spendingCeilings;
        }
    }
}
