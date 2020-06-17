using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Panda.App.Controllers;
using Panda.Domain;
using Panda.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Panda.Tests
{
    public class PackagesControllerTest
    {
        [Fact]
        public void IndexTest()
        {
            var packageServiceMock = new Mock<IPackagesService>();
            var userManagerServiceMock = TestUserManager<PandaUser>();
            var addressServiceMock = new Mock<IAddressesService>();
            var userServiceMock = new Mock<IUsersService>();

            var controller = new PackagesController
               (userManagerServiceMock, userServiceMock.Object, packageServiceMock.Object, addressServiceMock.Object);

            //packageServiceMock.Setup(x => x.GetAllPackages())
            //    .Returns(new Queryable<Package>() { });

            var result = controller.Create();
            Assert.IsType<ViewResult>(result);
        }

        public static UserManager<TUser> TestUserManager<TUser>(IUserStore<TUser> store = null) 
            where TUser : class
        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;
            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<TUser>>();
            var validator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<TUser>>();
            pwdValidators.Add(new PasswordValidator<TUser>());
            var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);
            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            return userManager;
        }
    }
}
