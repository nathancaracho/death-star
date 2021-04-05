using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;
using DeathStar.App.Core;
using System;

namespace DeathStar.App.Infrastructure.QueueRepository
{
    public class ServiceBusQueueRepository : IServiceBusQueueRepository
    {

        public async Task<int> Count(string connection, string queueName)
        {
            var managementClient = new ManagementClient(connection);
            var queue = await managementClient.GetQueueRuntimeInfoAsync($"{queueName}");
            return (int)queue.MessageCountDetails.DeadLetterMessageCount;

        }
    }
}