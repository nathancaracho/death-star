using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
namespace DeathStar.App.Infrastructure.QueueRepository
{
    public class ServiceBusQueueRepository : IServiceBusQueueRepository
    {

        public async Task<int> Count(string connection, string queueName)
        {
            await using (var client = new ServiceBusClient(connection))
            {
                var receiver = client.CreateReceiver(queueName);
                var count = receiver.PrefetchCount;
                return count;
            }
            return 1;
        }
    }
}