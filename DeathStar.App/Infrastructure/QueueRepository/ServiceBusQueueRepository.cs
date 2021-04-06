using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;
using DeathStar.App.Core;
using System;
using Azure.Messaging.ServiceBus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        public async Task<IEnumerable<ServiceBusReceivedMessage>> Pull(string connection, string queueName, int? count = null)
        {
            if (count.HasValue is false)
                count = await Count(connection, queueName);

            await using (var client = new ServiceBusClient(connection))
            {
                var deadLetter = client.CreateReceiver($"{queueName}/$DeadLetterQueue");
                return await deadLetter.ReceiveMessagesAsync(count.Value);
            }
        }
    }
}