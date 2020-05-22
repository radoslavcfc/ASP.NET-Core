using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Panda.Domain;
using Panda.Domain.Enums;
using Panda.Service;
using Panda.Services;
using PandaWeb.Models.Address;

namespace Panda_Job.Controllers
{
    public class AddressesController : Controller
    {
        private readonly UserManager<PandaUser> userManager;
        private readonly IAddressesService addressesService;
        private readonly IUsersService usersService;

        public AddressesController(UserManager<PandaUser> userManager, IAddressesService addressesService, IUsersService usersService)
        {
            this.userManager = userManager;
            this.addressesService = addressesService;
            this.usersService = usersService;
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

        [HttpPost]
        public IActionResult Create(AddNewAddressModel model)
        {
            return this.RedirectToAction("Preview", model);
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
            return this.View("Preview", model);
        }
        
        public IActionResult Save(AddNewAddressModel model)
        {
            var userId = this.userManager.GetUserId(this.User);
            var user = this.usersService.GetUserById(userId);

            if (user.Addresses.Count == 0)
            {
                model.AddressType = AddressType.Primary;
            }
            else
            {
                model.AddressType = AddressType.Alternative;
            }

            if (!ModelState.IsValid)
            {
                return this.View("Create");
            }

            var addresToRegister = new Address
            {
                Country = model.Country,
                Region = model.Region,
                Town = model.Town,
                StreetName = model.StreetName,
                AddressType = model.AddressType,
                PropertyType = model.PropertyType,
                Number = model.Number,
                UserId = this.userManager.GetUserId(this.User),
                
            };
            if (addresToRegister.PropertyType == PropertyType.Flat)
            {
                var flatToRegister = new Flat
                {
                   AddressId = addresToRegister.Id,
                   Entrance = model.FlatModel.Entrance,
                   Floor = model.FlatModel.Floor,
                   Apartment = model.FlatModel.Apartment

                };
                addresToRegister.Flat = flatToRegister;
            }
           

            this.addressesService.CreateAddress(addresToRegister);

            TempData["Message"] = "Done!";

            return this.View("Index", model);

            
        }
    }
}
