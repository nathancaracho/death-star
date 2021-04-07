using System.Collections.Generic;
using System.Threading.Tasks;
using DeathStar.App.Domain.Models;

namespace DeathStar.App.Infrastructure.FileRepository
{
    public interface IEnvironmentRepository
    {
        Task<IEnumerable<EnvironmentModel>> GetAll();
        Task SaveOne(EnvironmentModel environment);
        Task RemoveByName(string name);
        Task<EnvironmentModel> GetEnvironmentByName(string EnvName);
    }
}