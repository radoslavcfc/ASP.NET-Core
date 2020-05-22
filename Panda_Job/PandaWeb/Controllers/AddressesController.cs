﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Panda.Domain;
using PandaWeb.Models.Address;

namespace Panda_Job.Controllers
{
    public class AddressesController : Controller
    {
        private readonly UserManager<PandaUser> userManager;

        public AddressesController(UserManager<PandaUser> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return this.Ok();
        }

        [HttpGet]
        public IActionResult Create()
        {
           
               return this.View(new AddNewAddressModel());             
        }

        public IActionResult Create(AddNewAddressModel model)
        {
            var addresToRegister = new Address
            {
                Country = model.Country,
                Region = model.Region,
                Town = model.Town,
                StreetName = model.StreetName,
                AddressType = model.AddressType,
                PropertyType = model.PropertyType,
                Number = model.Number,
                Entrance = model.Entrance,
                Floor = model.Floor,
                Apartment = model.Apartment,
                UserId = this.userManager.GetUserId(this.User)
            };
            return this.RedirectToAction("Preview",addresToRegister);
        }
        [HttpGet]
        public IActionResult Preview(AddNewAddressModel model)
        {
            return this.View(model);
        }

        [HttpGet]
        public IActionResult Edit(AddNewAddressModel model)
        {
            return this.View("Create", model);
        }

        [HttpPost]
        [ActionName("Edit")]
        public IActionResult EditPost(AddNewAddressModel model)
        {
           
            return Ok(model.Country);
        }
    }
}
