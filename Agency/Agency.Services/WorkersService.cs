using Agency.Data.Models;
using Agency.Models.Workers;
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

        public string GetWorkerId(string userId)
        {
            var workerId = this._agencyDbContext.Workers
                .Where(worker => worker.AgencyUser.Id == userId)
                .Select(w => w.Id).FirstOrDefault();

            return workerId;
        }


        public async void UpdateAsync(string currentUserId, CompleteWorkerDataModel bindingModel)
        {
            var workerToUpdate = this._agencyDbContext
                .Workers
                .Where(w => w.AgencyUser.Id == currentUserId)
                .FirstOrDefault();

            workerToUpdate.CurrentlyIn = bindingModel.CurrentlyIn;
            workerToUpdate.AvailableFrom = bindingModel.AvailableFrom;
            workerToUpdate.AvailableTo = bindingModel.AvailableTo;
            workerToUpdate.ConnectionSource = bindingModel.ConnectionSource;
            workerToUpdate.DOB = bindingModel.DOB;
            workerToUpdate.Gender = bindingModel.Gender;
            workerToUpdate.Nationalities.Add(bindingModel.Nationality);
            workerToUpdate.Relatives = bindingModel.Relatives;

            this._agencyDbContext.Update(workerToUpdate);

            await this._agencyDbContext.SaveChangesAsync();
        }
    }
}
