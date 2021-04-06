using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeathStar.App.Infrastructure.QueueRepository
{
    public interface IQueueRepository<T>
    {
        Task<int> Count(string connection, string queueName);
        Task<IEnumerable<T>> Pull(string connection, string queueName, int? count = null);
    }
}