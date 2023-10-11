using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Service
{
    public interface IGoalService
    {
        Task<Guid> CreateGoalAsync(Goal goal);
        Task<List<Goal>> FilterGoalsAsync(Guid? id, Guid userId);
    }
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;
        public GoalService(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public async Task<Guid> CreateGoalAsync(Goal goal)
        {
            goal.Id = Guid.NewGuid();
            goal.CreatedAt = DateTime.Now;
            
            await _goalRepository.AddAsync(goal);
            
            return goal.Id;
        }

        public async Task<List<Goal>> FilterGoalsAsync(Guid? id, Guid userId)
        {
            var cards = await _goalRepository.FilterGoalAsync(id, userId);

            return cards;
        }

    }
}
