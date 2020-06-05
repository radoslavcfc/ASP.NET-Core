using Panda.Domain;
using System.Collections.Generic;

namespace Panda.Services
{
    public interface IUsersService
    {
        List<PandaUser> GetAllUsers();
        PandaUser GetUserById(string Id);
        PandaUser GetUserByName(string UserName);
        PandaUser GetUserByFullName(string fullName);
        void UpdateUser(string Id, PandaUser user);
    }
}
