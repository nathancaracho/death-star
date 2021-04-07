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
                ConsoleCore.Message("Saving env .....");
                var environment = new EnvironmentModel(Name, Connection, ShowWarning);
                await _fileRepository.SaveOne(environment);
                ConsoleCore.Success($"New env {Name} saved \n - {environment}");
            }
            catch (Exception ex)
            {
                ConsoleCore.Error($"Have some error when try save environment {ex.Message}");
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
            ConsoleCore.Message("Try find env .....");

            try
            {
                if (ShowAll)
                {
                    var environments = await _fileRepository.GetAll();
                    if (environments.Any() is false)
                        throw new ArgumentException("Have no environments saved");
                    ConsoleCore.Success(string.Join("\n ", environments));
                }
                else
                    ConsoleCore.Success((await _fileRepository.GetEnvironmentByName(Name)));

            }
            catch (Exception ex)
            {
                ConsoleCore.Error($"Have some error when try find environment : {ex.Message}");
                return -1;
            }

            return 1;
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
            ConsoleCore.Message("Removing env .....");
            try
            {
                await _fileRepository.RemoveByName(Name);
                ConsoleCore.Success($"Env {Name} was removed with success");
                return 1;
            }
            catch (Exception ex)
            {
                ConsoleCore.Error($"Can't Remove any environment : {ex}");
                return -1;
            }
        }
    }
}