using System.Threading.Tasks;
using DeathStar.App.Infrastructure.FileRepository;
using DeathStar.App.Infrastructure.QueueRepository;
using System.Linq;

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

        public async Task<int> Count(string envName, string queue)
        {
            var env = (await _fileRepository.GetAll()).FirstOrDefault(e=>e.Name.Equals(envName));
            var count = await _queueRepository.Count(env.Connection,queue);
            return count;
        }
    }
}