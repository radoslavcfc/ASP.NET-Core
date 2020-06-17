using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

using Panda.Data;
using Panda.Domain;
using Panda.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private void SeedTestData(PandaDbContext inMemoryContext)
        {
            inMemoryContext.Packages.AddRange(GetTestData());
            inMemoryContext.SaveChanges();
        }

        private void ClearData(PandaDbContext inMemoryContext)
        {
            if (inMemoryContext.Packages.Count() != 0)
            {
                inMemoryContext.Packages.RemoveRange(GetTestData());
                inMemoryContext.SaveChanges();
            }
           
        }

        //Actual Tests
        [Fact]
        public void TestGetAllUsers_WithoutAnyData()
        {
            var inMemoryContext = this.InitializeInMemoryDb();
            var packageService = new PackagesService(inMemoryContext);
            this.ClearData(inMemoryContext);
            var actualData = packageService.GetAllPackages().ToList();
            Assert.True(actualData.Count() == 0);

            inMemoryContext.Dispose();
        }

        [Fact]
        public void TestGetAllPackages_ShouldReturn_OnlyIfNotDeleted()
        {
            var inMemoryContext = this.InitializeInMemoryDb();
            var packageService = new PackagesService(inMemoryContext);
            var expectedData = GetTestData();
          
            this.SeedTestData(inMemoryContext);
            var actualData = packageService.GetAllPackages().ToList();

            Assert.NotEqual(expectedData.Count, actualData.Count);
            Assert.Single(actualData);
            foreach (var actualPackage in actualData)
            {
                Assert.True(expectedData.Any(user =>
                    actualPackage.Id == user.Id &&
                    actualPackage.ShippingAddress == user.ShippingAddress &&
                    actualPackage.Description == user.Description), "Wrong");
            }
            inMemoryContext.Dispose();
        }

        [Fact]
        public async Task TestGetAllUsers_UserDetailsMatch()
        {
            var inMemoryContext = this.InitializeInMemoryDb();
            var packageService = new PackagesService(inMemoryContext);
            this.ClearData(inMemoryContext);
            this.SeedTestData(inMemoryContext);
            var testPackageId = "TestId2";

            var expectedData = GetTestData()
                .SingleOrDefault(package =>
                     package.Id == testPackageId);

            var actualData = await packageService
                .GetPackageAsync(testPackageId);

            Assert.Equal(expectedData.Description, actualData.Description);
           
            inMemoryContext.Dispose();
        }

        [Fact]
        public async Task TestGetAllUsers_NonExistingUser()
        {
            var inMemoryContext = this.InitializeInMemoryDb();
            var packageService = new PackagesService(inMemoryContext);

          //  SeedTestData(inMemoryContext);
           
            string testUserId = "TestUser3";
            var actualData = await packageService
                .GetPackageAsync(testUserId);
            Assert.Null(actualData);
           
            inMemoryContext.Dispose();
        }
    }
}
