using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda_Job.Controllers
{
    public class AddressesController : Controller
    {
        public IActionResult Index()
        {
            return this.Ok();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.Ok();
        }

        public IActionResult Create(string a)
        {
            return this.Ok();
        }
    }
}
