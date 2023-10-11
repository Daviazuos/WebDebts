using MicroServices.WebDebts.Application.Models;
using MicroServices.WebDebts.Application.Models.DebtModels;
using MicroServices.WebDebts.Application.Models.Mappers;
using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Service;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroServices.WebDebts.Application.Services;

namespace MicroServices.WebDebts.Application.Service
{
    public interface IGoalsApplicationService
    {
        Task<GenericResponse> CreateGoal(GoalAppModel goalAppModel, Guid userId);
        Task<List<GoalResponse>> GetGoals(Guid userId);
    }

    public class GoalsApplicationService : IGoalsApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGoalService _goalService;
        private readonly IUserRepository _userRepository;
        private readonly IGoalRepository _goalRepository;
        private readonly IDebtsApplicationService _debtsApplicationService;

        public GoalsApplicationService(IUnitOfWork unitOfWork, IGoalService goalService, IGoalRepository goalRepository, IUserRepository userRepository, IDebtsApplicationService debtsApplicationService)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _goalRepository = goalRepository;
            _goalService = goalService;
            _debtsApplicationService = debtsApplicationService;
        }

        public async Task<GenericResponse> CreateGoal(GoalAppModel goalAppModel, Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            var goal = goalAppModel.ToEntity();

            goal.User = user;

            //var debtModel = new CreateDebtAppModel()
            //{
            //    CategoryId = '',
            //}

            //var debt = _debtsApplicationService.CreateDebt()

            await _goalService.CreateGoalAsync(goal);
            await _unitOfWork.CommitAsync();

            return new GenericResponse { Id = goal.Id };
        }

        public async Task<List<GoalResponse>> GetGoals(Guid userId)
        {

            return null;
        }
    }
}
