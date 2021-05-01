using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DeathStar.App.Core;
using DeathStar.App.SubCommands;
using DeathStar.App.Infrastructure.FileRepository;
using DeathStar.App.Infrastructure.QueueRepository;
using DeathStar.App.Domain.Services.Queue;

namespace DeathStar.App
{
    [Command(Description = "QUEUE Management"),
    Subcommand(typeof(EnvironmentSubCommand), typeof(ServiceBusSubCommand))]
    [HelpOption("-man")]
    class App : SubCommandBase
    {
        public static int Main(string[] args)
        {
            var services = new ServiceCollection()
                        .AddSingleton<IEnvironmentRepository, EnvironmentRepository>()
                        .AddScoped<IServiceBusQueueRepository, ServiceBusQueueRepository>()
                        .AddScoped<IServiceBusService, ServiceBusService>()
                        .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                    .BuildServiceProvider();
            var app = new CommandLineApplication<App>();

            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services);
            return app.Execute(args);
        }
    }
}
