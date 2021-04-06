using System.Threading.Tasks;
using DeathStar.App.Infrastructure.FileRepository;
using DeathStar.App.Infrastructure.QueueRepository;
using System.Linq;
using Azure.Messaging.ServiceBus;
using System;
using System.Text.Json;

namespace DeathStar.App.Domain.Services.Queue
{

    public class ServiceBusService : IServiceBusService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IQueueRepository<ServiceBusReceivedMessage> _queueRepository;
        public ServiceBusService(IFileRepository fileRepository, IServiceBusQueueRepository queueRepository)
        {
            _fileRepository = fileRepository;
            _queueRepository = queueRepository;
        }

        public async Task<int> Count(string envName, string queue)
        {
            var connection = await GetEnvConnection(envName);
            var count = await _queueRepository.Count(connection, queue);
            return count;
        }

        public async Task Pull(string envName, string queue, int? count = null)
        {
            var connection = await GetEnvConnection(envName);
            var messages = await _queueRepository.Pull(connection, queue, count);

            var parsedMessages = JsonSerializer.Serialize(messages.Select(message => JsonSerializer.Deserialize<object>(message.Body.ToString())));

            var fileName = $"{Environment.CurrentDirectory}/{queue}-{DateTime.Now.ToString("dd-MM-yy-mm-ss")}.json";
            await _fileRepository.Save(parsedMessages, fileName);
        }

        private async Task<string> GetEnvConnection(string envName)
        {
            var env = (await _fileRepository.GetAll()).FirstOrDefault(e => e.Name.Equals(envName));
            if (env is null)
                throw new ArgumentException($"Have no environment {envName}");
            return env.Connection;
        }
    }
}