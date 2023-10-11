using MicroServices.WebDebts.Domain.Interfaces.Repository;
using MicroServices.WebDebts.Domain.Models;
using MicroServices.WebDebts.Infrastructure.Database.Postgres;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly DataContext _context;
        private DbSet<User> _dbSet;

        public UserRepository(DataContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }

        public async Task<User> FindUserByUserPasswordAsync(string username, string password)
        {
            var user = await _dbSet.Where(x => x.Username == username && x.Password == password).FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("Usuário ou senha inválidos");
                
            return user;
        }
    }
}
