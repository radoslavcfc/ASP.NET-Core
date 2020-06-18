using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PandaWeb.Models.Error;

namespace Panda.App.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
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
