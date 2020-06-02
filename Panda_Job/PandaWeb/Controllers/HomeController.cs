using Microsoft.AspNetCore.Mvc;

using PandaWeb.Models.Package;

using Panda.Domain;
using Panda.Services;
using PandaWeb.Models.Address;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Docs.Samples;

namespace Panda.App.Controllers
{
 

    [Controller]
    public class HomeController : Controller
    {

        
        private readonly IUsersService usersService;

        public HomeController(IUsersService usersService, IAddressesService addressesService)
        {
            this.usersService = usersService;
            AddressesService = addressesService;
        }

        public IAddressesService AddressesService { get; }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var currentUserName = this.User.Identity.Name;
                var currentUser = this.usersService.GetUserByName(currentUserName);
                var currentUserAddressesCount = this.AddressesService.CountOfAddressesPerUser(currentUser);
                if (currentUserAddressesCount == 0)
                {
                    var addressModel = new AddNewAddressModel();
                    return this.View(addressModel);
                }
            }
            return this.View();
        }
    }
}
