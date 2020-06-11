using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Panda.Domain;
using Panda.Domain.Enums;
using Panda.Services;

using PandaWeb.Models.Address;
using PandaWeb.Models.Flat;
using System.Linq;

namespace Panda_Job.Controllers
{
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly UserManager<PandaUser> userManager;
        private readonly IAddressesService addressesService;
        private readonly IUsersService usersService;

        public AddressesController(UserManager<PandaUser> userManager,
                                    IAddressesService addressesService, 
                                    IUsersService usersService)
        {
            this.userManager = userManager;
            this.addressesService = addressesService;
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            var listOfAddresses = this.addressesService
                .ListOfAddressesByUser(this.User.Identity.Name).ToList();

            var model = new ListAddressesModel
            {
                ShortAddressDetailsModelsList = listOfAddresses
                .Select(a => new ShortAddressDetailModel
                {
                    Id = a.Id,
                    ShotenedContent = this.addressesService.ShortenedAddressToString(a),
                    AddressType = (AddressType)a.AddressType
                })
            };
            return this.View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new AddOrEditNewAddressModel();
            return this.View(model);
        }

        [HttpPost]
        [ActionName("Create")]
        public IActionResult CreatePost(AddOrEditNewAddressModel model)
        {
            return this.View("Preview", model);
        }

        public IActionResult Save(AddOrEditNewAddressModel model)
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

            TempData["SuccessCreatedAddress"] = "The address has been successfully saved!";

            return this.RedirectToAction("Index", "Addresses");
        }

     
        public ActionResult Delete(string id)
        {
            var addressFromDb = this.addressesService.GetAddressById(id);
            var model = new DeleteAddressModel
            {
                Id = id,
                ShortAddress = this.addressesService.ShortenedAddressToString(addressFromDb)
            };
            return this.View(model);
        }

        [HttpPost("Addresses/Delete")]
        
        public ActionResult Delete(DeleteAddressModel model)
        {
            var id = model.Id;
            this.addressesService.MarkAsDeleted(id);

            TempData["Deleted message"] = "The address was successfully deleted!";

            //return this.View("Deleted");
            return this.RedirectToAction("Index", "Addresses");
        }

        public ActionResult Edit(string id)
        {
            var address = this.addressesService.GetAddressById(id);
            var model = new AddOrEditNewAddressModel
            {
                Id = address.Id,
                Country = address.Country,
                Region = address.Region,
                Town = address.Town,
                StreetName = address.StreetName,
                AddressType = address.AddressType,
                PropertyType = address.PropertyType,
                Number = address.Number,
                
            };

            if (address.PropertyType == PropertyType.Flat)
            {
                var flatModel = new AddFlatModel
                {
                    
                    Entrance = model.FlatModel.Entrance,
                    Floor = model.FlatModel.Floor,
                    Apartment = model.FlatModel.Apartment
                };
                model.FlatModel = flatModel;
            }
            return this.View("Preview", model);
        }

        public ActionResult Edit(AddOrEditNewAddressModel model)
        {
            var idForUpdate = model.Id;
            var addressToUpdate = this.addressesService.GetAddressById(idForUpdate);
            if (!ModelState.IsValid)
            {
                return this.View("Edit");
            }
            //Could be done with Automapper, or even other function to be implemented
            //ToDo
            addressToUpdate.Country = model.Country;
            addressToUpdate.Region = model.Region;
            addressToUpdate.Town = model.Town;
            addressToUpdate.Number = model.Number;
            addressToUpdate.StreetName = model.StreetName;
            if (model.PropertyType == PropertyType.Flat)
            {
                addressToUpdate.Flat.Floor = model.FlatModel.Floor;
                addressToUpdate.Flat.Entrance = model.FlatModel.Entrance;
                addressToUpdate.Flat.Apartment = model.FlatModel.Apartment;
            }
            else 
            {

                addressToUpdate.Flat = null; 
            }

            TempData["SuccessEditAddress"] = "The address has been successfully edited and saved!";

            return this.RedirectToAction("Index", "Addresses");
        }
    }
}
