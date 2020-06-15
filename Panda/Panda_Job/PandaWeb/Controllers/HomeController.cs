using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panda.Services;
using PandaWeb.Models.Address;
using PandaWeb.Models.Error;
using System;
using System.Diagnostics;
using System.Security.Claims;

namespace Panda.App.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IAddressesService addressesService;

        public HomeController(IUsersService usersService, IAddressesService addressesService)
        {
            this.usersService = usersService;
            this.addressesService = addressesService;
        }
        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var currentUser = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = this.usersService.GetUserById(currentUser).Result;

                var currentUserAddressesCount = this.addressesService.CountOfAddressesPerUser(user);
                if (currentUserAddressesCount == 0)
                {
                    var addressModel = new AddOrEditNewAddressModel();
                    return this.View(addressModel);
                }
            }
            return this.View();
        }

        [AllowAnonymous]        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult StatusCode(string code)
        {
            var model = new StatusCodeModel();
            model.ErrorStatusCode = code;
            return this.View(model);
        }
    }
}
