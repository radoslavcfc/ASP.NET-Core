using Agency.Web.Data;
using System;
using System.Threading.Tasks;

namespace Agency.Data.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(AgencyDbContext dbContext, IServiceProvider serviceProvider);
    }
}
