using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Panda.Domain;
using Panda.Domain.Enums;
using Panda.Services;

using PandaWeb.Models.Address;
using PandaWeb.Models.Flat;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Save(AddOrEditNewAddressModel model)
        {
            var userId = this.userManager.GetUserId(this.User);
            var user = await this.usersService.GetUserByIdAsync(userId);

            if (this.addressesService.CountOfAddressesPerUser(user) == 0)
            {
                model.AddressType = AddressType.Primary;
            }

            else
            {
                model.AddressType = AddressType.Alternative;
            }

            if (model.PropertyType == PropertyType.House)
            {
                ModelState["FlatModel.Apartment"].ValidationState = ModelValidationState.Valid;
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

            await this.addressesService.CreateAddressAsync(addresToRegister);

            TempData["SuccessCreatedAddress"] = "The address has been successfully saved!";

            return this.RedirectToAction("Index", "Addresses");
        }

        public async Task<ActionResult> Delete(string id)
        {
            var addressFromDb = await this.addressesService.GetAddressByIdAsync(id);
            var model = new DeleteAddressModel
            {
                Id = id,
                ShortAddress = this.addressesService.ShortenedAddressToString(addressFromDb)
            };
            return this.View(model);
        }

        [HttpPost("Addresses/Delete")]

        public async Task<ActionResult> Delete(DeleteAddressModel model)
        {
            var id = model.Id;
            await this.addressesService.MarkAsDeletedAsync(id);

            TempData["Deleted message"] = "The address was successfully deleted!";

            //return this.View("Deleted");
            return this.RedirectToAction("Index", "Addresses");
        }

        public async Task<ActionResult> Edit(string id)
        {
            var address = await this.addressesService.GetAddressByIdAsync(id);
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
                model.FlatModel = new AddFlatModel();

                model.FlatModel.Entrance = address.Flat.Entrance;
                model.FlatModel.Floor = address.Flat.Floor;
                model.FlatModel.Apartment = address.Flat.Apartment;
            }
            return this.View("Preview", model);
        }

        public async Task<ActionResult> Update(AddOrEditNewAddressModel model)
        {
            var idForUpdate = model.Id;
            var addressToUpdate = await this.addressesService.GetAddressByIdAsync(idForUpdate);

            if (model.PropertyType == PropertyType.House)
            {
                ModelState["FlatModel.Apartment"].ValidationState = ModelValidationState.Valid;
            }
            if (!ModelState.IsValid)
            {
                return this.View("Preview",model);
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
                if (addressToUpdate.Flat == null)
                {
                    addressToUpdate.Flat = new Flat();
                    addressToUpdate.PropertyType = PropertyType.Flat;
                }

                addressToUpdate.Flat.Floor = model.FlatModel.Floor;
                addressToUpdate.Flat.Entrance = model.FlatModel.Entrance;
                addressToUpdate.Flat.Apartment = model.FlatModel.Apartment;
            }
            else
            {
                addressToUpdate.PropertyType = PropertyType.House;
                addressToUpdate.Flat = null;
            }
            await this.addressesService.UpdateAddressAsync(addressToUpdate);

            TempData["SuccessEditAddress"] = "The address has been successfully edited and saved!";

            return this.RedirectToAction("Index", "Addresses");
        }
    }
}
