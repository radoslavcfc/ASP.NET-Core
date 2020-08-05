
using Agency.Data.Models;
using Agency.Web.Data;
using System.Threading.Tasks;

namespace Agency.Services
{
    public class NamesService : INamesService
    {
        private readonly AgencyDbContext agencyDbContext;

        public NamesService(AgencyDbContext agencyDbContext)
        {
            this.agencyDbContext = agencyDbContext;
        }
        public async Task CreateNamesAsync()
        {
            var namesToAdd = new Names();
            await this.agencyDbContext.Names.AddAsync(namesToAdd);
        }
    }
}
