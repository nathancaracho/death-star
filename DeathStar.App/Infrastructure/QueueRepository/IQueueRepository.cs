using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeathStar.App.Infrastructure.QueueRepository
{
    public interface IQueueRepository<T>
    {
        Task<int> Count(string connection, string queueName);
        Task<IEnumerable<T>> PeekAll(string connection, string queueName);
        Task<IEnumerable<T>> ReceiveAll(string connection, string queueName);
        Task<T> PeekOne(string connection, string queueName);
        Task<IEnumerable<(string QueueName, long ActiveCount, long DlqCount)>> GetReport(string connection, string[] queueNames);
    }
}