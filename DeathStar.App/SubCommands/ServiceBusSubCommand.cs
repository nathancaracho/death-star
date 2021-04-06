using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DeathStar.App.Core;
using DeathStar.App.Domain.Services.Queue;
using McMaster.Extensions.CommandLineUtils;
using System;
namespace DeathStar.App.SubCommands
{
    [Command("asb-queue", Description = "Manage environments"),
        Subcommand(typeof(Count)), Subcommand(typeof(Pull))]
    public class ServiceBusSubCommand
    {

        private async Task<int> OnExecute(CommandLineApplication app)
        {

            ConsoleUtil.Message("count --env environment -q queue-name");
            ConsoleUtil.Message("pull --env environment -q queue-name --all -c 2");

            return 1;
        }

        [Command("count", Description = "Count deadletter queue message")]
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
                    ConsoleUtil.Message("Get DLQ Count.....");
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

        [Command("pull", Description = "Pull messages")]
        private class Pull : QueueSubCommand
        {
            [Range(1, 99999, ErrorMessage = "Count out of range")]
            [Option("--count", ShortName = "c", Description = "pull queue count")]
            private int? Count { get; set; }

            [Option("--all", Description = "Queue Name")]
            private bool PullAll { get; } = false;

            private readonly IServiceBusService _asbService;
            public Pull(IServiceBusService asbService)
            {
                _asbService = asbService;
            }

            private async Task<int> OnExecute()
            {

                try
                {
                    ConsoleUtil.Message("Pulling messages.....");
                    if (PullAll)
                        Count = 1;

                    await _asbService.Pull(EnvironmentName, QueueName, Count);
                    ConsoleUtil.Success($"The DLQ queue has been save");
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