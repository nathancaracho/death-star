using System.Threading.Tasks;
namespace DeathStar.App.Domain.Services.Queue
{
    public interface IQueueService
    {
        Task<int> Count(string envName, string queue);
    }
}