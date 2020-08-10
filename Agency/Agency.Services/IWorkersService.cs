using Agency.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Agency.Services
{
    public interface IWorkersService
    {
        Task AddAsync(Worker worker);
        string GetWorkerId(string userId);
    }
}
