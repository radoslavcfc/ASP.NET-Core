using Microsoft.EntityFrameworkCore;
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
            //CREATE PROCEDURE UsersOnlyInfo AS
            //SELECT U.[Id] ,U.[UserName] ,U.[NormalizedUserName],U.[Email] ,U.[NormalizedEmail] ,U.[EmailConfirmed] ,U.[PasswordHash],
            //U.[SecurityStamp] ,U.[ConcurrencyStamp] ,U.[PhoneNumber] ,U.[PhoneNumberConfirmed] ,U.[TwoFactorEnabled],U.[LockoutEnd],
            //U.[LockoutEnabled],U.[AccessFailedCount],U.[UserRoleId],U.[FirstName],U.[LastName],U.[SecondContactNumber] ,U.[RegisteredOn]
            //FROM[PandaDB].[dbo].[AspNetUsers] AS U
            //LEFT JOIN[PandaDB].[dbo].[AspNetUserRoles] AS UR
            //ON U.Id = UR.UserId
            //LEFT JOIN[PandaDB].[dbo].[AspNetRoles] AS R
            //ON UR.RoleId = r.Id
            //WHERE R.Name != 'Admin';

            var users = this.pandaDbContext.Users.FromSqlRaw("EXEC UsersOnlyInfo").ToList();
            return users;
        }

        public PandaUser GetUserById(string Id)
        {
            PandaUser userDb = this.pandaDbContext
                .Users
                .SingleOrDefault
                    (user => 
                        user.Id == Id);

            return userDb;
        }

        public PandaUser GetUserByUserName(string userName)
        {
            PandaUser userDb = this.pandaDbContext
                .Users
                .SingleOrDefault
                    (user => 
                        user.UserName == userName);
            
            return userDb;
        }

        public PandaUser GetUserByFullName(string fullName)
        {
            var firstName = fullName.Split(' ')[0];
            PandaUser userDb = this.pandaDbContext
                .Users
                .SingleOrDefault
                    (user =>
                        user.FirstName == firstName);

            return userDb;
        }
        public void UpdateUserInfo (PandaUser user)
        {
            try
            {
                this.pandaDbContext.Update(user);
                this.pandaDbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;
            };
        }

      
    }
}
