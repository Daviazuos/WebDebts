using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Infrastructure.Repositories
{
    public class DebtRepository : BaseRepository<Debt>, IDebtRepository
    {
        private readonly DataContext _context;
        private DbSet<Debt> _dbSet;
        private DbSet<Installments> _dbSetInstallment;

        public DebtRepository(DataContext context) : base(context)
        {
            _dbSet = context.Set<Debt>();
            _dbSetInstallment = context.Set<Installments>();
            _context = context;
        }

        public async Task DeleteDebt(Guid Id)
        {
            var debtToRemove = _dbSet.Where(x => x.Id == Id)
                                     .Include(x => x.Installments)
                                     .ToList();

            _dbSet.RemoveRange(debtToRemove);
            _dbSetInstallment.RemoveRange(debtToRemove.SelectMany(x => x.Installments));
        }

        public async Task<Debt> GetAllByIdAsync(Guid Id)
        {
            var resultQuery = _dbSet.Include(p => p.Installments)
                                    .Where(x => x.Id == Id).FirstOrDefault();

            resultQuery.Installments = resultQuery.Installments.OrderBy(x => x.Date).ToList();

            return resultQuery;
        }
    }
}
