using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Infrastructure.Repositories
{
    public class ResponsiblePartyRepository : BaseRepository<ResponsibleParty>, IResponsiblePartyRepository
    {
        private readonly DataContext _context;
        private DbSet<ResponsibleParty> _dbSet;

        public ResponsiblePartyRepository(DataContext context) : base(context)
        {
            _dbSet = context.Set<ResponsibleParty>();
            _context = context;
        }

        public async Task<List<ResponsibleParty>> GetByUserId(Guid userId)
        {
            return await _dbSet.Where(x => x.User.Id == userId).ToListAsync();
        }
    }

}

