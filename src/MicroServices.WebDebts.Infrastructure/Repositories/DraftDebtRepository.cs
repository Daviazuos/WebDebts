using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Domain.Models.Commom;
using MicroServices.WebDebts.Domain.Models.Enum;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using MicroServices.WebDebts.Infrastructure.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Infrastructure.Repositories
{
    public class DraftDebtRepository : BaseRepository<DraftDebt>, IDraftDebtRepository
    {
        private readonly DataContext _context;
        private DbSet<DraftDebt> _dbSet;


        public DraftDebtRepository(DataContext context) : base(context)
        {
            _dbSet = context.Set<DraftDebt>();
            _context = context;
        }

        public async Task<List<DraftDebt>> GetByUserIdAsync(Guid userId)
        {
            var resultQuery = _dbSet.Include(x => x.Card).Where(x => x.User.Id == userId);

            return resultQuery.ToList();
        }
    }
}
