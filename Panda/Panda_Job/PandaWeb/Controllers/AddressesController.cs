using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

using Panda.Domain;
using Panda.Domain.Enums;
using Panda.Services;
using PandaWeb.Models.Address;
using PandaWeb.Models.Flat;


namespace Panda_Job.Controllers
{
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly UserManager<PandaUser> _userManager;
        private readonly IAddressesService _addressesService;
        private readonly IUsersService _usersService;
        private readonly ILogger<AddressesController> _logger;
        public AddressesController(UserManager<PandaUser> userManager, IAddressesService addressesService,
            IUsersService usersService, ILogger<AddressesController> logger)
        {
            this._userManager = userManager;
            this._addressesService = addressesService;
            this._usersService = usersService;
            this._logger = logger;
        }
        public IActionResult Index()
        {
            var listOfAddresses = this._addressesService
                .ListOfAddressesByUser(this.User.Identity.Name).ToList();

            if (listOfAddresses == null)
            {
                _logger.LogWarning($"Get(listOfAddresses) from DB - NOT FOUND");
                return this.NotFound();
            }

            var model = new ListAddressesModel
            {
                ShortAddressDetailsModelsList = listOfAddresses
                .Select(a => new ShortAddressDetailModel
                {
                    Id = a.Id,
                    ShotenedContent = this._addressesService.ShortenedAddressToString(a),
                    AddressType = (AddressType)a.AddressType
                })
            };
            return this.View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new UpdateAddressModel();
            return this.View(model);
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(UpdateAddressModel model)
        {
            return this.View("Preview", model);
        }

        [HttpPost]    
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(UpdateAddressModel model)
        {
            var userId = this._userManager.GetUserId(this.User);
            var user = await this._usersService.GetUserByIdAsync(userId);

            if (user == null)
            {
                _logger.LogWarning($"Get(currentUser) from DB - NOT FOUND");
                return this.NotFound();
            }

            if (this._addressesService.CountOfAddressesPerUser(user) == 0)
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
                _logger.LogWarning($"Model State invalid for {nameof(UpdateAddressModel)}");
                return this.View("Create");
            }

            var addressToRegister = new Address
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

            if (addressToRegister.PropertyType == PropertyType.Flat)
            {
                var flatToRegister = new Flat
                {
                    AddressId = addressToRegister.Id,
                    Entrance = model.FlatModel.Entrance,
                    Floor = model.FlatModel.Floor,
                    Apartment = model.FlatModel.Apartment
                };
                addressToRegister.Flat = flatToRegister;
            }

            
            await this._addressesService.CreateAddressAsync(addressToRegister);

            _logger.LogInformation($"Address with id {addressToRegister.Id} has been registered for user with id{user.Id}");
            TempData["SuccessCreatedAddress"] = "The address has been successfully saved!";

            return this.RedirectToAction("Index", "Addresses");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var address = await this._addressesService
                .GetAddressByIdAsync(id);

            if (address == null)
            {
                _logger.LogWarning($"Get(address) with {id} from DB - NOT FOUND");
                return this.NotFound();
            }

            var model = new UpdateAddressModel
            {
                Id = address.Id,
                Country = address.Country,
                Region = address.Region,
                Town = address.Town,
                StreetName = address.StreetName,
                AddressType = address.AddressType,
                PropertyType = address.PropertyType,
                Number = address.Number
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateAddressModel model)
        {
            var idForUpdate = model.Id;
            var addressToUpdate = await this._addressesService
                .GetAddressByIdAsync(idForUpdate);

            if (addressToUpdate == null)
            {
                _logger.LogWarning($"Get(address) with {model.Id} from DB - NOT FOUND");
                return this.NotFound();
            }

            if (model.PropertyType == PropertyType.House)
            {
                ModelState["FlatModel.Apartment"].ValidationState = ModelValidationState.Valid;
            }

            if (!ModelState.IsValid)
            {
                return this.View("Preview",model);
            }
           
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
                //TODO flat delete from Db
            }
            await this._addressesService.UpdateAddressAsync(addressToUpdate);

            _logger.LogInformation($"Address with {addressToUpdate.Id} for user with {addressToUpdate.Id} has been updated");

            TempData["SuccessEditAddress"] = "The address has been successfully edited and saved!";

            return this.RedirectToAction("Index", "Addresses");
        }

        [HttpGet]
       public async Task<IActionResult> Delete(string id)
        {
            var addressFromDb = await this._addressesService
                .GetAddressByIdAsync(id);

            if (addressFromDb == null)
            {
                _logger.LogWarning($"Get(address) with {id} from DB - NOT FOUND");
                return this.NotFound();
            }
            var model = new DeleteAddressModel
            {
                Id = id,
                ShortAddress = this._addressesService
                    .ShortenedAddressToString(addressFromDb)
            };
            return this.View(model);
        }

        [HttpPost("Addresses/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteAddressModel model)
        {
            if (model == null)
            {
                _logger.LogWarning($"Binding model with {model.Id} - NOT FOUND");
                return this.NotFound();
            }

            var id = model.Id;
            await this._addressesService.MarkAsDeletedAsync(id);
            _logger.LogInformation($"Address with {id} has been marked as deleted");

            TempData["Deleted message"] = "The address was successfully deleted!";
         
            return this.RedirectToAction("Index", "Addresses");
        }
    }
}
