using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DeathStar.App.Domain.Models;

namespace DeathStar.App.Infrastructure.FileRepository
{
    public class EnvironmentRepository : IEnvironmentRepository
    {
        private readonly string _fileEnvName = "env.json";
        public async Task<IEnumerable<EnvironmentModel>> GetAll()
        {
            if (File.Exists(_fileEnvName) is false)
                return Enumerable.Empty<EnvironmentModel>();

            using StreamReader file = new(_fileEnvName);
            var environments = await file.ReadToEndAsync();
            return JsonSerializer.Deserialize<IEnumerable<EnvironmentModel>>(environments);
        }
        public async Task SaveOne(EnvironmentModel environment)
        {
            var environments = await GetAll();
            await SaveEnvironment(environments.Append(environment));
        }

        public async Task RemoveByName(string name)
        {
            var environments = await GetAll();
            await SaveEnvironment(environments.Where(env => env.Name != name));
        }

        private async Task SaveEnvironment(IEnumerable<EnvironmentModel> environments)
        {
            using StreamWriter writer = new(_fileEnvName);
            await writer.WriteAsync(JsonSerializer.Serialize(environments));
        }

        public async Task<EnvironmentModel> GetEnvironmentByName(string envName)
        {
            var environment = (await GetAll()).FirstOrDefault(env => env.Name.Equals(envName));
            if (environment is null)
                throw new ArgumentException($"Have no environment with name {envName}");
            return environment;
        }
    }
}