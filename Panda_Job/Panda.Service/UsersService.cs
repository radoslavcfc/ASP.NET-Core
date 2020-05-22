using Panda.Data;
using Panda.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Panda.Services
{
    public class UsersService : IUsersService
    {
        private readonly PandaDbContext pandaDbContext;

        public UsersService(PandaDbContext pandaDbContext)
        {
            this.pandaDbContext = pandaDbContext;
        }

        public List<PandaUser> GetAllUsers()
        {
            List<PandaUser> users = this.pandaDbContext.Users.ToList();

            return users;
        }

        public PandaUser GetUserById(string Id)
        {
            PandaUser userDb = this.pandaDbContext.Users.SingleOrDefault(user => user.Id == Id);

            return userDb;
        }

        public PandaUser GetUserByName(string userName)
        {
            PandaUser userDb = this.pandaDbContext.Users.SingleOrDefault(user => user.UserName == userName);

            return userDb;
        }
        
      
    }
}
