using Agency.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Agency.Services
{
    public interface IWorkersService
    {
        Task AddAsync(Worker worker);
        IQueryable GetWorkerId(string userId);
    }
}
