using Microsoft.EntityFrameworkCore;
using Panda.Data;
using Panda.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public class PackagesService : IPackagesService
    {
        private readonly PandaDbContext pandaDbContext;

        public PackagesService(PandaDbContext pandaDbContext)
        {
            this.pandaDbContext = pandaDbContext;
        }

        public async Task CreatePackage(Package package)
        {
            await this.pandaDbContext.Packages
                .AddAsync(package);
            await this.pandaDbContext.SaveChangesAsync();
        }

        public async Task<Package> GetPackage(string id)
        {
            Package package = await this.pandaDbContext
                .Packages
                .Where(packageDb => packageDb.Id == id
                            && packageDb.IsDeleted == false)
                .Include(packageDb => packageDb.Recipient)
                .FirstOrDefaultAsync();

            return package;
        }
        public async Task UpdatePackage(Package package)
        {
            this.pandaDbContext.Update(package);
            await this.pandaDbContext.SaveChangesAsync();
        }

        public IQueryable<Package> GetAllPackages()
        {
            var collection = this.pandaDbContext
                .Packages
                .Where(p => p.IsDeleted == false);
            return collection;
        }
    }
}
