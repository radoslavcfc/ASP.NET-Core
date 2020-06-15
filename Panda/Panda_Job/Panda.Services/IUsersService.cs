using Panda.Domain;
using System.Threading.Tasks;

namespace Panda.Services
{
    public interface IUsersService
    {
        //List<PandaUser> GetAllUsersNoAdmins();
        PandaUser GetUserById(string Id);
        PandaUser GetUserByUserName(string UserName);
        PandaUser GetUserByFullName(string fullName);
        void UpdateUserInfo(PandaUser user);
        Task SaveToDataBaseAsync();
        Task<bool> DeleteAccountAsync(PandaUser currentUser);
    }
}
