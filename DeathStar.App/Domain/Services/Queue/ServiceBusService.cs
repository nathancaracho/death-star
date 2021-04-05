using System.Threading.Tasks;
using DeathStar.App.Infrastructure.FileRepository;
using DeathStar.App.Infrastructure.QueueRepository;

namespace DeathStar.App.Domain.Services.Queue
{

    public class ServiceBusService : IServiceBusService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IQueueRepository _queueRepository;
        public ServiceBusService(IFileRepository fileRepository, IServiceBusQueueRepository queueRepository)
        {
            _fileRepository = fileRepository;
            _queueRepository = queueRepository;
        }

        public Task<int> Count(string envName, string queue)
        {
            return Task.FromResult(1);
        }
    }
}