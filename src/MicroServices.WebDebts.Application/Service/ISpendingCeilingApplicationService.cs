using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Models.Mappers;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Service;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Application.Service
{
    public interface ISpendingCeilingApplicationService
    {
        Task<GenericResponse> CreateSpendingCeiling(SpendingCeilingAppModel spendingCeilingAppModel, Guid userId);
        Task<List<SpendingCeilingResponse>> GetSpendingCeiling(Guid userId, int month, int year);
    }

    public class SpendingCeilingApplicationService : ISpendingCeilingApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpendingCeilingService _spendingCeilingService;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDebtRepository _debtRepository;
        
        public SpendingCeilingApplicationService(IUnitOfWork unitOfWork, ISpendingCeilingService spendingCeilingService, IUserRepository userRepository, ICategoryRepository categoryRepository, IDebtRepository debtRepository)
        {
            _unitOfWork = unitOfWork;
            _spendingCeilingService = spendingCeilingService;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _debtRepository = debtRepository;
        }

        public async Task<GenericResponse> CreateSpendingCeiling(SpendingCeilingAppModel spendingCeilingAppModel, Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            var debtCategory = await _categoryRepository.FindByIdAsync(spendingCeilingAppModel.CategoryId);

            var spendingCeiling = spendingCeilingAppModel.ToEntity();

            spendingCeiling.Id = Guid.NewGuid(); 
            spendingCeiling.DebtCategory = debtCategory;
            spendingCeiling.User = user;


            await _spendingCeilingService.CreateSpendingCeiling(spendingCeiling);
            await _unitOfWork.CommitAsync();

            return new GenericResponse { Id = spendingCeiling.Id };
        }

        public async Task<List<SpendingCeilingResponse>> GetSpendingCeiling(Guid userId, int month, int year)
        {

            var spendingCeilings = await _spendingCeilingService.GetSpendingCeilingByMonth(userId, month, year);
            var response = new List<SpendingCeilingResponse>();

            foreach (var spendingCeiling in spendingCeilings)
            {
                var debts = await _debtRepository.GetSumByCategoryMonth(month, year, userId, spendingCeiling.DebtCategory.Id);
                var sumDebtsValue = debts.Select(x => x.Installments.Sum(x => x.Value)).Sum();

                response.Add(new SpendingCeilingResponse
                {
                    CategoryName = spendingCeiling.DebtCategory.Name,
                    Date = spendingCeiling.Date,
                    PercentValue = Math.Round(sumDebtsValue / spendingCeiling.Value * 100, 2),
                    DebtValue = sumDebtsValue,
                    SpendingCeilingValue = spendingCeiling.Value
                });
            }           

            return response;
        }
    }
}
