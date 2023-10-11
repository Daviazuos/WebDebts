using MicroServices.WebDebts.Domain.Models;
using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> FindUserByUserPasswordAsync(string username, string password);

    }
}
