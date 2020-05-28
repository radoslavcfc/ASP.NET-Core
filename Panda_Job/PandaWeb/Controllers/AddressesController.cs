using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Panda.Domain;
using Panda.Domain.Enums;
using Panda.Service;
using Panda.Services;
using PandaWeb.Models.Address;
using System.Linq;

namespace Panda_Job.Controllers
{
    public class AddressesController : Controller
    {
        private readonly UserManager<PandaUser> userManager;
        private readonly IAddressesService addressesService;
        private readonly IUsersService usersService;

        public AddressesController(UserManager<PandaUser> userManager,
                                    IAddressesService addressesService, IUsersService usersService)
        {
            this.userManager = userManager;
            this.addressesService = addressesService;
            this.usersService = usersService;
        }
              public IActionResult Index()
        {
            var listOfAddresses = this.addressesService
                .ListOfAddressesByUser(this.User.Identity.Name);

            var model = new ListAddressesModel
            {
                ShortAddressDetailsModelsList = listOfAddresses
                     .Select(a => new ShortAddressDetailModel
                     {
                         Country = a.Country.Substring(0, 3).ToUpper(),
                         Region = a.Region.Substring(0, 3).ToUpper(),
                         Town = a.Town.ToUpper(),
                         StreetName = a.StreetName,
                         AddressType = a.AddressType.Value,
                         Number = a.Number
                     })
                     .OrderBy(a => a.AddressType)
            };
            return this.View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new AddNewAddressModel();
            return this.View(model);
        }

        [HttpPost]
        [ActionName("Create")]
        public IActionResult CreatePost(AddNewAddressModel model)
        {
            return this.View("Preview",model);          
        }

        public IActionResult Save(AddNewAddressModel model)
        {          
            var userId = this.userManager.GetUserId(this.User);
            var user = this.usersService.GetUserById(userId);

            if (this.addressesService.CountOfAddressesPerUser(user) == 0)
            {
                model.AddressType = AddressType.Primary;
            }
            else
            {
                model.AddressType = AddressType.Alternative;
            }

            //The model state is checked after the system allocate the type of the address automatically

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
                UserId = userId

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

            return this.RedirectToAction("Index", "Addresses");
        }
    }
}
