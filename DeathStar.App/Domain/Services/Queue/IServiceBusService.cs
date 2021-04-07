using System.Threading.Tasks;

namespace DeathStar.App.Domain.Services.Queue
{
    public interface IServiceBusService : IQueueService
    {
        Task<int> Count(string envName, string queue);
    }
}