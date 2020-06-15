using Microsoft.EntityFrameworkCore;
using Panda.Data;
using Panda.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public class UsersService : IUsersService
    {
        private readonly PandaDbContext pandaDbContext;

        public UsersService(PandaDbContext pandaDbContext)
        {
            this.pandaDbContext = pandaDbContext;
        }

        public async Task<PandaUser> GetUserByIdAsync(string Id)
        {
            PandaUser userDb = await this.pandaDbContext
                .Users
                .Include(p => p.Addresses)
                .FirstOrDefaultAsync
                    (user =>
                        user.Id == Id &&
                        user.IsDeleted == false);

            return userDb;
        }

        public async Task UpdateUserInfoAsync(PandaUser user)
        {
            this.pandaDbContext.Update(user);
            await this.pandaDbContext.SaveChangesAsync();
        }

        public async Task SaveToDataBaseAsync()
        {
            await this.pandaDbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAccountAsync(PandaUser currentUser)
        {
            var user = await this.pandaDbContext.Users
                .Where(u => u.Id == currentUser.Id).FirstOrDefaultAsync();

            user.IsDeleted = true;
            user.UserName = null;
            user.PasswordHash = null;
            user.NormalizedUserName = null;

            await this.pandaDbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// This method was implemented with purely experimentally purpose
        /// I just wanted to write my own way to get all the users, who are not admins =>
        /// equivalent to the UserManager.GetUsersInRoleAsync("admin");
        /// </summary>
        /// <returns></returns>
        public IQueryable<PandaUser> GetAllUsersNoAdmins()
        {
            //CREATE OR ALTER PROCEDURE UsersOnlyInfo AS
            //SELECT U.[Id] ,U.[UserName] ,U.[NormalizedUserName],U.[Email] ,U.[NormalizedEmail] ,U.[EmailConfirmed] ,U.[PasswordHash],
            //U.[SecurityStamp] ,U.[ConcurrencyStamp] ,U.[PhoneNumber] ,U.[PhoneNumberConfirmed] ,U.[TwoFactorEnabled],U.[LockoutEnd],
            //U.[LockoutEnabled],U.[AccessFailedCount],U.[UserRoleId],U.[FirstName],U.[LastName],U.[SecondContactNumber] ,U.[RegisteredOn]
            //FROM[PandaDB].[dbo].[AspNetUsers] AS U
            //LEFT JOIN[PandaDB].[dbo].[AspNetUserRoles] AS UR
            //ON U.Id = UR.UserId
            //LEFT JOIN[PandaDB].[dbo].[AspNetRoles] AS R
            //ON UR.RoleId = r.Id
            //WHERE R.Name != 'Admin' AND U.IsDeleted = 0;

            var users = this.pandaDbContext.Users
                .FromSqlRaw("EXEC UsersOnlyInfo");
            return users;
        }
    }
}
