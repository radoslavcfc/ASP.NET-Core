using System.Linq;
using System.Threading.Tasks;

using Panda.Domain;

namespace Panda.Services
{
    public interface IUsersService
    {
        IQueryable<PandaUser> GetAllUsersNoAdmins();
        Task<PandaUser> GetUserByIdAsync(string Id);
              
        Task UpdateUserInfoAsync(PandaUser user);
        Task SaveToDataBaseAsync();
        Task<bool> DeleteAccountAsync(PandaUser currentUser);
        Task <PandaUser> GetUserByIdWithDeletedAsync(string id);
        Task DeleteAllDataForUserAsync(string id);
        Task<string> GetRoleByIdAsync(string id);
    }
}
