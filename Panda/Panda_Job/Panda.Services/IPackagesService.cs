using Panda.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public interface IPackagesService
    {
        Task CreatePackageAsync(Package package);

        Task<Package> GetPackageAsync(string id);

        IQueryable<Package> GetAllPackages();

        Task UpdatePackageAsync(Package package);
        IEnumerable<Package> GetAllPackagesWithStatusForUser(string currentUserId, string status);
        IEnumerable<Package> GetAllPackagesWithStatusForAdmin(string status);
    }
}
