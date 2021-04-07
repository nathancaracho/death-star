using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DeathStar.App.Core;
using DeathStar.App.Domain.Services.Queue;
using McMaster.Extensions.CommandLineUtils;
using System;
using DeathStar.App.Infrastructure.FileRepository;

namespace DeathStar.App.SubCommands
{
    [Command("asb-queue", Description = "Manage queue"),
        Subcommand(typeof(Count)), Subcommand(typeof(Pull))]
    [HelpOption("-man")]
    public class ServiceBusSubCommand : SubCommandBase
    {
        [Command("count", Description = "Count deadletter queue message")]
        [HelpOption("-man")]
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
                    ConsoleCore.Message("Get DLQ Count.....");
                    var count = await _asbService.Count(EnvironmentName, QueueName);
                    ConsoleCore.Success($"The DLQ queue {QueueName} have {count} itens for env {EnvironmentName}.");
                    return 1;
                }
                catch (Exception ex)
                {
                    ConsoleCore.Error($"Have some error whe try get messages count {ex.Message}");
                    return -1;
                }
            }
        }

        [Command("pull", Description = "Pull messages")]
        [HelpOption("-man")]
        private class Pull : QueueSubCommand
        {
            [Range(1, 99999, ErrorMessage = "Count out of range")]
            [Option("--count", ShortName = "c", Description = "pull queue count")]
            private int? Count { get; set; }

            [Option("--all", Description = "pull all menssages")]
            private bool PullAll { get; } = false;

            private readonly IEnvironmentRepository _environmentRepository;
            private readonly IServiceBusService _asbService;
            public Pull(IServiceBusService asbService, IEnvironmentRepository environmentRepository)
            {
                _environmentRepository = environmentRepository;
                _asbService = asbService;
            }

            private async Task<int> OnExecute()
            {

                try
                {

                    var environment = await _environmentRepository.GetEnvironmentByName(EnvironmentName);

                    var keep = !environment.ShowWarning || ConsoleCore.GetYesNo("If you pull DLQ messages you will download and delete, do you wanna make it?");
                    if (keep is false)
                        return 1;

                    ConsoleCore.Message("Pulling messages.....");
                    if (PullAll is false)
                        Count = 1;

                    await _asbService.Pull(environment.Connection, QueueName, Count);
                    ConsoleCore.Success($"The DLQ queue has been saved");
                    return 1;
                }
                catch (Exception ex)
                {
                    ConsoleCore.Error($"Have some error when try pull messages: {ex.Message}");
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