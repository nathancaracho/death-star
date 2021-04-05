using System.Threading.Tasks;

namespace DeathStar.App.Infrastructure.QueueRepository
{
    public interface IQueueRepository
    {
        Task<int> Count();
    }
}