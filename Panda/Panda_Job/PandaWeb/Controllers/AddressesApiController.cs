using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Panda.Services;
using PandaWeb.Models.Address;
using PandaWeb.Models.User;

namespace PandaWeb.Controllers
{
    [ApiController]
    public class AddressesApiController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IAddressesService _addressesService;
        private readonly ILogger<AddressesApiController> _logger;

        public AddressesApiController(IUsersService usersService, IAddressesService addressesService, 
            ILogger<AddressesApiController> logger)
        {
            this._usersService = usersService;
            this._addressesService = addressesService;
            this._logger = logger;
        }

        [HttpPost]
        [Route("addresses/api/fetch", Order = 1)]
        public async Task<ActionResult<ListOfAddresesWithCountModel>> AddressesFetch(UserApiModel inputModel)
        {
            var currentUser =  await this._usersService
                .GetUserByIdAsync(inputModel.Id);           

            if (currentUser == null)
            {
                _logger.LogWarning($"Get current user from DB - NOT FOUND");
                return this.NotFound();
            }

            var count = _addressesService
               .CountOfAddressesPerUser(currentUser);

            var listModel = new ListOfAddresesWithCountModel();
            listModel.CountOfAddresses = count;

            var addressesPerUser = _addressesService
                .ListOfAddressesByUser(currentUser.UserName);

            if (addressesPerUser == null)
            {
                _logger.LogWarning($"Get list of addresses for user {currentUser.Id} from DB - NOT FOUND");
                return this.NotFound();
            }

            if (addressesPerUser.Count() > 0)
            {
                foreach (var address in addressesPerUser)
                {
                    var shortAddress = new ApiAddressModel
                    {
                        Id = address.Id,
                        Country = address.Country,
                        Region = address.Region,
                        Town = address.Town,
                        StreetName = address.StreetName,
                        Number = address.Number,
                        AddressType = address.AddressType.Value
                    };
                    listModel.ListOfAdresses.Add(shortAddress);
                }
            }
            return listModel;
        }
    }
}
