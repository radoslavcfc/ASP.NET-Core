using Microsoft.AspNetCore.Mvc;
using PandaWeb.Models.Address;
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
            var model = new AddNewAddressModel();
            return this.View(model);
        }

       

        public IActionResult Create(string a)
        {
            return this.Ok();
        }
    }
}
