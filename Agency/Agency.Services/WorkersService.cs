using Agency.Data.Models;
using Agency.Web.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Agency.Services
{
    public class WorkersService : IWorkersService
    {
        private readonly AgencyDbContext _agencyDbContext;

        public WorkersService(AgencyDbContext agencyDbContext)
        {
            this._agencyDbContext = agencyDbContext;
        }
        public async Task AddAsync(Worker worker)
        {
            await this._agencyDbContext.Workers.AddAsync(worker);
            await this._agencyDbContext.SaveChangesAsync();
        }
    }
}
