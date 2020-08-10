using Agency.Data.Models;
using Agency.Web.Data;

using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Agency.Services
{
    public class WorkersService : IWorkersService
    {
        private readonly AgencyDbContext _agencyDbContext;
        private readonly UserManager<AgencyUser> _userManager;

        public WorkersService(AgencyDbContext agencyDbContext, UserManager<AgencyUser> userManager)
        {
            this._agencyDbContext = agencyDbContext;
            this._userManager = userManager;
        }
        public async Task AddAsync(Worker worker)
        {
            await this._agencyDbContext.Workers.AddAsync(worker);
            await this._agencyDbContext.SaveChangesAsync();
        }

        public IQueryable GetWorkerId(string userId)
        {
            var workerId = this._agencyDbContext.Workers
                .Where(worker => worker.AgencyUser.Id == userId)
                .Select(w => w.Id);

            return workerId;
        }
    }
}
