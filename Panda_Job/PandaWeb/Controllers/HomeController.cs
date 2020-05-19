using Microsoft.AspNetCore.Mvc;

using PandaWeb.Models.Package;

using Panda.Domain;
using Panda.Services;
using PandaWeb.Models.Address;
using System.Collections.Generic;
using System.Linq;

namespace Panda.App.Controllers
{
    [Controller]
    public class HomeController : Controller
    {

        private readonly IPackagesService packagesService;
        private readonly IUsersService usersService;

        public HomeController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var currentUserName = this.User.Identity.Name;
                var currentUser = this.usersService.GetUserByName(currentUserName);
                if (currentUser.Addresses.Count == 0)
                {
                    var addressModel = new AddNewAddressModel();
                    return this.View(addressModel);
                }
            }
            return this.View();
        }
    }
}
