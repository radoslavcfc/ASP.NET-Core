using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Panda.Services;
using PandaWeb.Models.Address;
using PandaWeb.Models.User;
using System.Linq;

namespace PandaWeb.Controllers
{
    [ApiController]
    public class AddressesApiController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IAddressesService addressesService;

        public AddressesApiController(IUsersService usersService, IAddressesService addressesService)
        {
            this.usersService = usersService;
            this.addressesService = addressesService;
        }

        [HttpPost]
        [Route("addresses/api/fetch", Order = 1)]
        public ActionResult<ListOfAddresesWithCountModel> AddressesFetch(UserApiModel inputModel)
        {
            var currentUser = this.usersService.GetUserById(inputModel.Id).Result;
            var count = addressesService.CountOfAddressesPerUser(currentUser);

            var listModel = new ListOfAddresesWithCountModel();
            listModel.CountOfAddresses = count;
            var addressesPerUser = addressesService.ListOfAddressesByUser(currentUser.UserName);

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
