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
    [Command("queue", Description = "Manage queue"),
        Subcommand(typeof(Dlq))]
    [HelpOption("-?")]
    public class ServiceBusSubCommand : SubCommandBase
    {
        [Command("dlq", Description = "Dead Letter"),
            Subcommand(typeof(Count)), Subcommand(typeof(Peek))]
        [HelpOption("-?")]
        private class Dlq : SubCommandBase
        {
            [Command("count", Description = "Count deadletter queue message")]
            [HelpOption("-?")]
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

            [Command("peek", Description = "Peek messages")]
            [HelpOption("-?")]
            private class Peek : QueueSubCommand
            {
                [Option("--all", Description = "peek all menssages")]
                private bool PeekAll { get; } = false;

                private readonly IEnvironmentRepository _environmentRepository;
                private readonly IServiceBusService _asbService;
                public Peek(IServiceBusService asbService, IEnvironmentRepository environmentRepository)
                {
                    _environmentRepository = environmentRepository;
                    _asbService = asbService;
                }

                private async Task<int> OnExecute()
                {

                    try
                    {

                        var environment = await _environmentRepository.GetEnvironmentByName(EnvironmentName);

                        ConsoleCore.Message("Peeking messages...");

                        await _asbService.Peek(environment.Connection, QueueName, PeekAll);
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