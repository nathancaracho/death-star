using System.Threading.Tasks;
using DeathStar.App.Infrastructure.FileRepository;
using DeathStar.App.Infrastructure.QueueRepository;
using System.Linq;
using Azure.Messaging.ServiceBus;
using System;
using System.Text.Json;
using System.IO;
using DeathStar.App.Core;
using System.Collections.Generic;

namespace DeathStar.App.Domain.Services.Queue
{

    public class ServiceBusService : IServiceBusService
    {
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IQueueRepository<ServiceBusReceivedMessage> _queueRepository;
        public ServiceBusService(IEnvironmentRepository fileRepository, IServiceBusQueueRepository queueRepository)
        {
            _environmentRepository = fileRepository;
            _queueRepository = queueRepository;
        }

        public async Task<int> Count(string envName, string queue)
        {
            var connection = await GetConnection(envName);
            return await _queueRepository.Count(connection, queue);
        }

        public async Task Peek(string envName, string queue, bool peekAll)
        {
            var connection = await GetConnection(envName);
            var fileName = $"{Environment.CurrentDirectory}/peek-{envName}-{queue}-{DateTime.Now.ToString("dd-MM-yy-mm-ss")}.json";

            IEnumerable<ServiceBusReceivedMessage> messages = Enumerable.Empty<ServiceBusReceivedMessage>();
            if (peekAll)
                messages = await _queueRepository.PeekAll((string)connection, queue);
            else
                messages.Append(await _queueRepository.PeekOne((string)connection, queue));

            ConsoleCore.Message("Serializing message...");
            var parsedMessages = JsonSerializer.Serialize(messages.Select(message => JsonSerializer.Deserialize<object>(message.Body.ToString())));

            ConsoleCore.Message("Saving...");
            using StreamWriter writer = new(fileName);
            await writer.WriteAsync(parsedMessages);
        }

        public async Task Receive(string envName, string queue)
        {
            var connection = await GetConnection(envName);
            var fileName = $"{Environment.CurrentDirectory}/receive-{envName}-{queue}-{DateTime.Now.ToString("dd-MM-yy-mm-ss")}.json";

            IEnumerable<ServiceBusReceivedMessage> messages = Enumerable.Empty<ServiceBusReceivedMessage>();

            messages = await _queueRepository.ReceiveAll(connection, queue);


            ConsoleCore.Message("Serializing message...");
            var parsedMessages = JsonSerializer.Serialize(messages.Select(message => JsonSerializer.Deserialize<object>(message.Body.ToString())));

            ConsoleCore.Message("Saving...");
            using StreamWriter writer = new(fileName);
            await writer.WriteAsync(parsedMessages);
        }

        private async Task<string> GetConnection(string envName) =>
            (await _environmentRepository.GetEnvironmentByName(envName)).Connection;
    }
}