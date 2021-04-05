using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DeathStar.App.Core;
using DeathStar.App.Infrastructure.FileRepository;
using DeathStar.App.Infrastructure.QueueRepository;
using McMaster.Extensions.CommandLineUtils;

namespace DeathStar.App.SubCommands
{
    [Command("queue-sb", Description = "Manage environments")]
    public class ServiceBusSubCommand
    {
        private readonly IFileRepository _fileRepository;

        [Required(ErrorMessage = "The Environment name is required")]
        [Option("--env", Description = "Environment Name")]
        public string EnvironmentName { get; }

        [Required(ErrorMessage = "The Queue name is required")]
        [Option("--queue", ShortName = "q", Description = "Queue Name")]
        public string QueueName { get; }
        public ServiceBusSubCommand(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        private async Task<int> On(CommandLineApplication app)
        {
            var environment = (await _fileRepository.GetAll()).FirstOrDefault(env => env.Name.Equals(EnvironmentName));
            if (environment is null)
                ConsoleUtil.Error($"The env {EnvironmentName} not exists");
            var queueRepository = new ServiceBusQueueRepository(environment.Connection, $"{QueueName}/$deadLetter");
            
            app.HelpOption(inherited: true);
            
            app.Command("count", config =>
            {   
                config.OnExecuteAsync(_ =>
                {
                    var count = queueRepository.Count();
                    ConsoleUtil.Success($"The {QueueName} has {count} itens");
                    return Task.FromResult(1);
                });
            });
            ConsoleUtil.Error("You must specify an action. See --help for more details.");
            return 1;
        }

    }


}