using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Panda.App.Controllers;
using Panda.Domain;
using Panda.Domain.Enums;
using Panda.Services;
using PandaWeb.Models.Package;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace Panda.Tests
{
    public class PackagesControllerTest
    {
        public PackagesController InitializeControllerContstuctor()
        {
            var packageServiceMock = new Mock<IPackagesService>();
            var userManagerServiceMock = TestUserManager<PandaUser>();
            var addressServiceMock = new Mock<IAddressesService>();
            var userServiceMock = new Mock<IUsersService>();

            var controller = new PackagesController
               (userManagerServiceMock, userServiceMock.Object, packageServiceMock.Object, addressServiceMock.Object);

            controller.ControllerContext = this.InitializeHttpContextWithAdmin();
            return controller;
        }

        private ControllerContext InitializeHttpContextWithAdmin()
        {
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.IsInRole("admin")).Returns(true);

            var context = new ControllerContext(
                new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));
            return context;
        }

        [Fact]
        public void PackagesController_IndexTest()
        {
            var controller = this.InitializeControllerContstuctor();

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.IsInRole("admin")).Returns(true);
           
            var context = new ControllerContext(
                new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));

            controller.ControllerContext = context;
           
            var result = controller.Index();
            Assert.IsType<ViewResult>(result);

            //httpContext.Setup(m => m.User.IsInRole("user")).Returns(true);
            //var resultWithUserRole = controller.Index();
            //Assert.IsType<ViewResult>(resultWithUserRole);

        }

        [Fact]
        public void PackagesController_IndexTest_CountOfPackages()
        {
            var mockPakcageService = new Mock<IPackagesService>();
            var userManagerServiceMock = TestUserManager<PandaUser>();
            var addressServiceMock = new Mock<IAddressesService>();
            var userServiceMock = new Mock<IUsersService>();

            var controller = new PackagesController
               (userManagerServiceMock, userServiceMock.Object, mockPakcageService.Object, addressServiceMock.Object);

            mockPakcageService.Setup(list => list.GetAllPackages())
                .Returns(this.GetSampleListPackage());

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.IsInRole("admin")).Returns(true);
            var context = new ControllerContext(
                new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));
            controller.ControllerContext = context;

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            //Controller transforming the result from the service to List<PackageHomeViewModel>(), after mapping it
            var model = Assert.IsAssignableFrom<List<PackageHomeViewModel>> (
            viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

            [Fact]
        public void PackagesController_CreateTest()
        {
            var controller = this.InitializeControllerContstuctor();

            var result = controller.Create();
            Assert.IsType<ViewResult>(result);
        }

        private IQueryable<Package> GetSampleListPackage()
        {
            var list = new List<Package>();
            list.Add(new Package { Id = "1", Description = "1.1" });
            list.Add(new Package { Id = "2", Description = "2.1" });

            return list.AsQueryable<Package>();
        }
        private static UserManager<TUser> TestUserManager<TUser>(IUserStore<TUser> store = null) 
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
