using System.Threading.Tasks;

namespace MicroServices.WebDebts.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
