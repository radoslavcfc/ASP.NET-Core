using Microsoft.EntityFrameworkCore;
using Panda.Data;
using Panda.Domain;
using Panda.Domain.Enums;
using System.Collections;
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

        public async Task CreatePackageAsync(Package package)
        {
            await this.pandaDbContext.Packages
                .AddAsync(package);
            await this.pandaDbContext.SaveChangesAsync();
        }

        public async Task<Package> GetPackageAsync(string id)
        {
            Package package = await this.pandaDbContext
                .Packages
                .Where(packageDb => packageDb.Id == id
                            && packageDb.IsDeleted == false)
                .Include(packageDb => packageDb.Recipient)
                .FirstOrDefaultAsync();

            return package;
        }
        public async Task UpdatePackageAsync(Package package)
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

        public IEnumerable<Package> GetAllPackagesWithStatusForUser(string currentUserId,string status)
        {
            var collection = this.pandaDbContext
                .Packages
                .AsEnumerable()
                .Where(p =>
                     p.RecipientId == currentUserId &&
                     p.Status.ToString().ToLower() == status &&
                     p.IsDeleted == false); 
            return collection;
        }
    
        public IEnumerable<Package> GetAllPackagesWithStatusForAdmin(string status)
        {
            var collection = this.pandaDbContext
                .Packages
                 .AsEnumerable()
                .Where(p => p.Status.ToString()
                                    .ToLower() == status);
                
            return collection;
        }
    }
}
