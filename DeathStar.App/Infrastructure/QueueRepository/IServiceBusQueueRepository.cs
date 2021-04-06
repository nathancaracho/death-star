using Azure.Messaging.ServiceBus;

namespace DeathStar.App.Infrastructure.QueueRepository
{
    public interface IServiceBusQueueRepository : IQueueRepository<ServiceBusReceivedMessage>
    {


    }
}