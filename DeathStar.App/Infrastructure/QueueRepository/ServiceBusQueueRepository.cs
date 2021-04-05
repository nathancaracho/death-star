using System.Threading.Tasks;

namespace DeathStar.App.Infrastructure.QueueRepository
{
    public class ServiceBusQueueRepository : QueueRepository, IQueueRepository
    {
        public ServiceBusQueueRepository(string connection, string queueName) : base(connection, queueName)
        {
        }

        public Task<int> Count()
        {
            return Task.FromResult(1);
        }
    }
}