using Agency.Web.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Data.Seeding
{
    public class AgencyDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(AgencyDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

           
            var seeders = new List<ISeeder>
                          {
                              //new RolesSeeder(),
                              
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
               // logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}
