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
    public class PackageServiceTest
    {
        private PandaDbContext InitializeInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<PandaDbContext>()
                .UseInMemoryDatabase(databaseName: "pandaDbContextInMemory").Options;

            var inMemoryContext = new PandaDbContext(options);
            //SeedTestData(inMemoryContext);
            return inMemoryContext;
        }
        private List<Package> GetTestData()
        {
            return
            new List<Package>
            {
                new Package
                {
                    Id = "TestId1",
                    Description = "Test1",
                    Weight = 1.2,
                    ShippingAddress = "TestAddress1",
                    IsDeleted = true
                    
                },
                new Package
                {
                    Id = "TestId2",
                    Description = "Test2",
                    Weight = 1.2,
                    ShippingAddress = "TestAddress2",
                    IsDeleted = false
                }
            };
        }

        private void SeedTestData(PandaDbContext context)
        {
            context.Packages.AddRange(GetTestData());
            context.SaveChanges();
        }

        //Actual Tests
        [Fact]
        public void TestGetAllPackagesShouldReturn()
        {
            var inMemoryContext = this.InitializeInMemoryDb();
            var packageService = new PackagesService(inMemoryContext);
            var expectedData = GetTestData();
            var actualData = packageService.GetAllPackages().ToList();

            Assert.NotEqual(expectedData.Count, actualData.Count);
            Assert.Single(expectedData);
            foreach (var actualPackage in actualData)
            {
                Assert.True(expectedData.Any(user =>
                    actualPackage.Id == user.Id &&
                    actualPackage.ShippingAddress == user.ShippingAddress &&
                    actualPackage.Description == user.Description), "Wrong");
            }
        }

        [Fact]
        public void TestGetAllUsers_WithoutAnyData()
        {
            var options = new DbContextOptionsBuilder<PandaDbContext>()
               .UseInMemoryDatabase(databaseName: "GetAllUsers").Options;

            var context = new PandaDbContext(options);
            SeedTestData(context);
            var usersService = new UsersService(context);
            var actualData = usersService.GetAllUsersNoAdmins();
            Assert.True(actualData.Count() == 0); 
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
          //  Assert.Equal(expectedData.UserName, actualData.Result.UserName);
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
            var actualData = usersService.GetUserByIdAsync(testUserId).Result;
            Assert.Null(actualData);
        }

       
    }
}
