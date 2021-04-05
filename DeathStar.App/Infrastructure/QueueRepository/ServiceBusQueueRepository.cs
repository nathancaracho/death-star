using System.Threading.Tasks;

namespace DeathStar.App.Infrastructure.QueueRepository
{
    public class ServiceBusQueueRepository : IServiceBusQueueRepository
    {

        public Task<int> Count(string connection, string queueName)
        {
            return Task.FromResult(1);
        }
    }
}