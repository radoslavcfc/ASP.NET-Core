using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

using Panda.Data;
using Panda.Domain;
using Panda.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Panda.Tests
{
    public class UserServiceTests
    {

        //TODO
        //All the testing needs redoing
       
        [Fact]
        public void TestGetAllUsersShouldReturn()
        {
            var options = new DbContextOptionsBuilder<PandaDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllUsers").Options;

            var context = new PandaDbContext(options);
            SeedTestData(context);
            var usersService = new UsersService(context);
            var expectedData = GetTestData();
           // var actualData = usersService.;
          
          //  Assert.Equal(expectedData.Count, actualData.Count);
            Assert.Equal(2, expectedData.Count);
           //foreach (var actualUser in actualData)
           // {
           //     Assert.True(expectedData.Any(user =>
           //         actualUser.UserName == user.UserName &&
           //         actualUser.UserRole.Name == user.UserRole.Name &&
           //         actualUser.Email == user.Email), "Wrong");
           // }
        }

        [Fact]
        public void TestGetAllUsers_WithoutAnyData()
        {
            var options = new DbContextOptionsBuilder<PandaDbContext>()
               .UseInMemoryDatabase(databaseName: "GetAllUsers").Options;

            var context = new PandaDbContext(options);
            //SeedTestData(context);
            var usersService = new UsersService(context);
       //     var actualData = usersService.GetAllUsersNoAdmins();
       //     Assert.True(actualData.Count==0); 
        }

        [Fact]
        public void TestGetAllUsers_UserDetailsMatch()
        {
            var options = new DbContextOptionsBuilder<PandaDbContext>()
               .UseInMemoryDatabase(databaseName: "GetAllUsers").Options;            
            var context = new PandaDbContext(options);
            SeedTestData(context);
            var usersService = new UsersService(context);
            var testUserId = "TestId1";
            var expectedData = GetTestData().SingleOrDefault(user =>
            user.Id == testUserId);
            var actualData = usersService.GetUserByIdAsync(testUserId);
          //  Assert.Equal(expectedData.UserName, actualData.UserName);
           
        }

        [Fact]
        public void TestGetAllUsers_NonExistanceUser()
        {
            var options = new DbContextOptionsBuilder<PandaDbContext>()
               .UseInMemoryDatabase(databaseName: "GetAllUsers").Options;

            var context = new PandaDbContext(options);
            SeedTestData(context);
            var usersService = new UsersService(context);
            string testUserId = "TestUser3";
            var actualData = usersService.GetUserByIdAsync(testUserId);
            Assert.Null(actualData);
        }

        private List<PandaUser> GetTestData() 
        { 
            return
            new List<PandaUser>
            {
                new PandaUser
                {
                    Id = "TestId1",
                    UserName = "TestUser1",
                    Email = "abv@abv.bg",
                    UserRole = new PandaUserRole(){Name = "Admin"}
                },
                new PandaUser
                {
                    Id = "TestId2",
                    UserName = "TestUser2",
                    Email = "bva@bva.gb",
                    UserRole = new PandaUserRole(){Name = "User"}
                }
            };
        }

        private void SeedTestData(PandaDbContext context)
        {
            context.Users.AddRange(GetTestData());
            context.SaveChanges();
        }
    }
}
