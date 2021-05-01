using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;
using DeathStar.App.Core;
using System;
using Azure.Messaging.ServiceBus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DeathStar.App.Domain.Models;

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

        public async Task<IEnumerable<ServiceBusReceivedMessage>> PeekAll(string connection, string queueName)
        {

            var queueLength = await Count(connection, queueName);
            long fromSequenceNumber = 0;
            List<ServiceBusReceivedMessage> messages = new List<ServiceBusReceivedMessage>();

            ConsoleCore.ProgressBar(messages.Count, queueLength);


            await using (var client = new ServiceBusClient(connection))
            {
                var deadLetter = client.CreateReceiver($"{queueName}/$DeadLetterQueue");
                do
                {
                    var peekedMessages = await deadLetter.PeekMessagesAsync(queueLength, fromSequenceNumber);

                    if (peekedMessages.Any())
                    {
                        messages.AddRange(peekedMessages);
                        fromSequenceNumber = peekedMessages
                                            .OrderByDescending(msg => msg.EnqueuedSequenceNumber)
                                            .Last()
                                            .SequenceNumber;

                        ConsoleCore.ProgressBar(messages.Count, queueLength);
                    }
                    else
                        break;

                } while (messages.Count < queueLength);

            }
            return messages;
        }

        public async Task<ServiceBusReceivedMessage> PeekOne(string connection, string queueName)
        {


            await using (var client = new ServiceBusClient(connection))
            {
                var deadLetter = client.CreateReceiver($"{queueName}/$DeadLetterQueue");

                return await deadLetter.PeekMessageAsync();

            }
        }
    }
}