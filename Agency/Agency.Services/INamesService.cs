using Agency.Data.Models;
using System.Threading.Tasks;

namespace Agency.Services
{
   public interface INamesService
    {
        Task AddAsync(Names namesOfUser);
    }
}
