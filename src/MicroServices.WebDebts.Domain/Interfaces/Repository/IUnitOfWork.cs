using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
