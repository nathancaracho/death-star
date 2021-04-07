using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DeathStar.App.Core;
using DeathStar.App.Domain.Models;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Linq;
using DeathStar.App.Infrastructure.FileRepository;

namespace DeathStar.App.SubCommands
{
    [Command("env", Description = "Manage environments"),
        Subcommand(typeof(Save), typeof(List), typeof(Remove))]
    [HelpOption("-man")]
    public class EnvironmentSubCommand : SubCommandBase
    {

    }

    [Command("save", Description = "Save an environment")]
    [HelpOption("-man")]
    class Save
    {

        [Required(ErrorMessage = "The environment name is required")]
        [Option("--name", ShortName = "n", Description = "Environment Name")]
        public string Name { get; }

        [Required(ErrorMessage = "The connection string is required")]
        [Option("--connection", ShortName = "cs", Description = "Connection string")]
        public string Connection { get; }

        [Option("--warning", ShortName = "w", Description = "Show warning when try some action with this env")]
        public bool ShowWarning { get; }

        private readonly IEnvironmentRepository _fileRepository;
        public Save(IEnvironmentRepository fileRepository) => _fileRepository = fileRepository;

        public async Task<int> OnExecute()
        {
            try
            {
                ConsoleUtil.Message("Saving env .....");
                await _fileRepository.SaveOne(new EnvironmentModel(Name, Connection, ShowWarning));
                ConsoleUtil.Success($"New env {Name} saved with success");
            }
            catch (Exception ex)
            {
                ConsoleUtil.Error($"Env is not saved : {ex}");
                return -1;
            }

            return 1;
        }
    }

    [Command("list", Description = "list an environment")]
    [HelpOption("-man")]
    class List
    {
        [Option("--name", ShortName = "n", Description = "Environment Name")]
        public string Name { get; }

        [Option("--all", Description = "List all environments")]
        public bool ShowAll { get; }

        private readonly IEnvironmentRepository _fileRepository;
        public List(IEnvironmentRepository fileRepository) => _fileRepository = fileRepository;
        public async Task<int> OnExecute()
        {
            ConsoleUtil.Message("Try find env .....");

            try
            {
                var environments = await _fileRepository.GetAll();
                if (environments.Any() is false)
                    ConsoleUtil.Warning("Have no environments saved");

                if (ShowAll)
                    environments.ToList().ForEach(writeEnvironment);
                else
                {
                    var environment = environments.FirstOrDefault(env => env.Name.Equals(Name));
                    if (environment is null)
                    {
                        ConsoleUtil.Warning($"Have no environments saved with this name: {Name}");
                        return -1;
                    }

                    writeEnvironment(environment);
                }

            }
            catch (Exception ex)
            {
                ConsoleUtil.Error($"Can't find any environment : {ex}");
                return -1;
            }

            return 1;

            void writeEnvironment(EnvironmentModel env) => ConsoleUtil.Success($"Name: {env.Name}, Connection: {env.Connection}, Show warning: {env.ShowWarning}");
        }
    }

    [Command("Remove", Description = "remove an environment")]
    [HelpOption("-man")]
    class Remove
    {
        [Option("--name", ShortName = "n", Description = "Environment Name")]
        public string Name { get; }

        private readonly IEnvironmentRepository _fileRepository;
        public Remove(IEnvironmentRepository fileRepository) => _fileRepository = fileRepository;
        public async Task<int> OnExecute()
        {
            ConsoleUtil.Message("Removing env .....");
            try
            {
                await _fileRepository.RemoveByName(Name);
                ConsoleUtil.Success($"Env {Name} was removed with success");
                return 1;
            }
            catch (Exception ex)
            {
                ConsoleUtil.Error($"Can't Remove any environment : {ex}");
                return -1;
            }
        }
    }
}