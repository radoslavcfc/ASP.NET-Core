using Agency.Data.Models;
using System.Threading.Tasks;
using Agency.Models.Workers;

namespace Agency.Services
{
    public interface IWorkersService
    {
        Task AddAsync(Worker worker);
        string GetWorkerId(string userId);
        void UpdateAsync(string currentUserId, CompleteWorkerDataModel bindingModel);
    }
}
