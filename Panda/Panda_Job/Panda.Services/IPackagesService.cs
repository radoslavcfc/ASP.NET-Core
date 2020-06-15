using Panda.Domain;

using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public interface IPackagesService
    {
        Task CreatePackage(Package package);

        Task<Package> GetPackage(string id);

        IQueryable<Package> GetAllPackages();

        Task UpdatePackage(Package package);
               
    }
}
