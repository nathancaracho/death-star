using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DeathStar.App.Core;
using DeathStar.App.Domain.Services.Queue;
using McMaster.Extensions.CommandLineUtils;
using System;
namespace DeathStar.App.SubCommands
{
    [Command("asb-queue", Description = "Manage environments"), Subcommand(typeof(Count))]
    public class ServiceBusSubCommand
    {

        private async Task<int> OnExecute(CommandLineApplication app)
        {

            ConsoleUtil.Error("You must specify an action. See --help for more details.");
            return 1;
        }

        [Command("count", Description = "Manage environments")]
        private class Count : QueueSubCommand
        {
            private readonly IServiceBusService _asbService;
            public Count(IServiceBusService asbService)
            {
                _asbService = asbService;
            }

            private async Task<int> OnExecute()
            {
                try
                {
                    var count = await _asbService.Count(EnvironmentName, QueueName);
                    ConsoleUtil.Success($"The DLQ queue {QueueName} have {count} itens for env {EnvironmentName}.");
                    return 1;
                }
                catch (Exception ex)
                {
                    ConsoleUtil.Error($"Have some error whe try get messages count {ex}");
                    return -1;
                }
            }
        }
    }


    class QueueSubCommand
    {
        [Required(ErrorMessage = "The Environment name is required")]
        [Option("--env", Description = "Environment Name")]
        protected string EnvironmentName { get; }

        [Required(ErrorMessage = "The Queue name is required")]
        [Option("--queue", ShortName = "q", Description = "Queue Name")]
        protected string QueueName { get; }
    }

}