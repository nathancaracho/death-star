using System.Collections.Generic;
using System.Threading.Tasks;
using DeathStar.App.Domain.Models;

namespace DeathStar.App.Infrastructure.FileRepository
{
    public interface IFileRepository
    {
        Task<IEnumerable<EnvironmentModel>> GetAll();
        Task SaveOne(EnvironmentModel environment);
        Task RemoveByName(string name);
    }
}