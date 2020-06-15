using Panda.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public interface IUsersService
    {
        IQueryable<PandaUser> GetAllUsersNoAdmins();
        Task<PandaUser> GetUserByIdAsync(string Id);
              
        Task UpdateUserInfoAsync(PandaUser user);
        Task SaveToDataBaseAsync();
        Task<bool> DeleteAccountAsync(PandaUser currentUser);
        PandaUser GetUserById(string Id);
    }
}
