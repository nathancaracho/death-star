using System.Threading.Tasks;
using DeathStar.App.Infrastructure.FileRepository;
using DeathStar.App.Infrastructure.QueueRepository;
using System.Linq;
using Azure.Messaging.ServiceBus;
using System;
using System.Text.Json;
using System.IO;
using DeathStar.App.Core;

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

        public async Task Peek(string connection, string queue, bool peekAll, int? count = null)
        {
            var fileName = $"{Environment.CurrentDirectory}/{queue}-{DateTime.Now.ToString("dd-MM-yy-mm-ss")}.json";

            var messages = await _queueRepository.PeekAll((string)connection, queue);

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