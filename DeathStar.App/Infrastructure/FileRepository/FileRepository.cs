using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DeathStar.App.Domain.Models;

namespace DeathStar.App.Infrastructure.FileRepository
{
    public class FileRepository : IFileRepository
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
            => await Save(JsonSerializer.Serialize(environments), _fileEnvName);

        public async Task Save(string file, string filePath)
        {
            using StreamWriter writer = new(_fileEnvName);
            await writer.WriteAsync(file);
        }


    }
}