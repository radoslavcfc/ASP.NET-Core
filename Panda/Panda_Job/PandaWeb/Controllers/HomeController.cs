using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panda.Services;
using PandaWeb.Models.Address;
using PandaWeb.Models.Error;
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
