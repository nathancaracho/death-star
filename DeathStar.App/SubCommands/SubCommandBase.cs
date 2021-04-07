using System.Threading.Tasks;
using DeathStar.App.Core;
using McMaster.Extensions.CommandLineUtils;

namespace DeathStar.App.SubCommands
{
    public class SubCommandBase
    {
        protected async Task<int> OnExecute(CommandLineApplication app)
        {

            ConsoleCore.Title();
            app.ShowHelp();
            return 1;
        }
    }
}