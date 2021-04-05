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
        private readonly string _fileName = "env.json";
        public async Task<IEnumerable<EnvironmentModel>> GetAll()
        {
            if (File.Exists(_fileName) is false)
                return Enumerable.Empty<EnvironmentModel>();

            using StreamReader file = new(_fileName);
            var environments = await file.ReadToEndAsync();
            return JsonSerializer.Deserialize<IEnumerable<EnvironmentModel>>(environments);
        }
        public async Task SaveOne(EnvironmentModel environment)
        {
            var environments = await GetAll();
            await Save(environments.Append(environment));
        }

        public async Task RemoveByName(string name)
        {
            var environments = await GetAll();
            await Save(environments.Where(env => env.Name != name));
        }
        private async Task Save(IEnumerable<EnvironmentModel> environments)
        {
            using StreamWriter file = new(_fileName);
            await file.WriteAsync(JsonSerializer.Serialize(environments));
        }
    }
}