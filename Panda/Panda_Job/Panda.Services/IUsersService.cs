using Panda.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public interface IUsersService
    {
        IQueryable<PandaUser> GetAllUsersNoAdmins();
        Task<PandaUser> GetUserById(string Id);
              
        Task UpdateUserInfo(PandaUser user);
        Task SaveToDataBaseAsync();
        Task<bool> DeleteAccountAsync(PandaUser currentUser);
    }
}
