using System.Threading.Tasks;
namespace DeathStar.App.Domain.Services.Queue
{
    public interface IQueueService
    {
        Task<int> Count(string envName, string queue);
        Task Pull(string connection, string queue, int? count = null);

    }
}