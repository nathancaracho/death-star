using System.Linq;
using DeathStar.App.Domain.Models;


namespace DeathStar.App.Infrastructure.QueueRepository
{
    public class QueueRepository
    {
        protected readonly string _connection;
        protected readonly string _queueName;

        public QueueRepository(string connection, string queueName)
        {
            _connection = connection;
            _queueName = queueName;
        }

    }
}