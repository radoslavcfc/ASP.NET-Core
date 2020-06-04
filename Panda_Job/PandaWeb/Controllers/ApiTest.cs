using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Panda.Services;
using PandaWeb.Models.Address;
using PandaWeb.Models.User;
using System.Linq;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace PandaWeb.Controllers
{
    [ApiController]
    public class AddressesApi : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IAddressesService addressesService;

        public AddressesApi(IUsersService usersService, IAddressesService addressesService)
        {
            this.usersService = usersService;
            this.addressesService = addressesService;
        }

        [HttpPost]
        [Route("api/test", Order = 1)]
        public ActionResult<ListOfAddresesWithCountModel> MyApiTest(UserApiModel inputModel)
        {
            var currentUserFullName = inputModel.FullName;
            var currentUser = this.usersService.GetUserByFullName(currentUserFullName);
            var count = addressesService.CountOfAddressesPerUser(currentUser);

            var listModel = new ListOfAddresesWithCountModel();
            listModel.CountOfAddresses = count;
            var addressesPerUser = addressesService.ListOfAddressesByUser(currentUser.UserName);

            if (addressesPerUser.Count() > 0)
            {
                foreach (var address in addressesPerUser)
                {
                    var shortAddress = new ShortAddressDetailModel
                    {
                        Country = address.Country,
                        Region = address.Region,
                        Town = address.Town

                    };
                    listModel.ListOfAdresses.Add(shortAddress);
                }
            }
            //var result = JsonConvert.SerializeObject(listModel);
            return listModel;
        }

        [HttpPost]
        [Route("api/test", Order = 2)]
        public ActionResult MyApiTestManualJson(UserApiModel model)
        {
            var currentUserFullName = model.FullName;
            var currentUser = this.usersService.GetUserByFullName(currentUserFullName);
            var count = addressesService.CountOfAddressesPerUser(currentUser);

            string jsonResult = $"{{count:{count},";
            var addressesPerUser = addressesService.ListOfAddressesByUser(currentUser.UserName);

            if (addressesPerUser.Count() > 0)
            {
                jsonResult += "addresses:[";
            }

            foreach (var address in addressesPerUser)
            {
                jsonResult += $"{{country:{address.Country}, " +
                                 $"region:{address.Region}, " +
                                 $"town:{address.Town}, " +
                                 $"streetName:{address.StreetName}, " +
                                 $"number:{address.Number}}},";
            }
            jsonResult += "]";
            return this.Ok(jsonResult);
        }
    }
}
