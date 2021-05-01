using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeathStar.App.Infrastructure.QueueRepository
{
    public interface IQueueRepository<T>
    {
        Task<int> Count(string connection, string queueName);
        Task<IEnumerable<T>> PeekAll(string connection, string queueName);
    }
}