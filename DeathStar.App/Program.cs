using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DeathStar.App.Core;
using DeathStar.App.SubCommands;
using DeathStar.App.Infrastructure.FileRepository;
namespace DeathStar.App
{
    [Command(Description = "Queue pull and push"), Subcommand(typeof(EnvironmentSubCommand), typeof(ServiceBusSubCommand))]
    class App
    {
        public static async Task<int> Main(string[] args)
        {
            var services = new ServiceCollection()
                        .AddSingleton<IFileRepository, FileRepository>()
                        .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                    .BuildServiceProvider();
            var app = new CommandLineApplication<App>();

            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services);
            return app.Execute(args);
        }

        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            ConsoleUtil.Title();
            app.ShowHelp();
            return 1;
        }
    }
}
