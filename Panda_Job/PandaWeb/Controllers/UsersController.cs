using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panda.Services;
using PandaWeb.Models.User;

namespace PandaWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IAddressesService addressesService;

        public UsersController(IUsersService usersService, IAddressesService addressesService)
        {
            this.usersService = usersService;
            this.addressesService = addressesService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var allUsersFromDb = usersService.GetAllUsers();
            var model = new AllUsersIndexViewModel();
            foreach (var singleUser in allUsersFromDb)
            {
                var modelUser = new UserIndexViewModel
                {
                    FullName = this.FullNameCreator(singleUser.FirstName, singleUser.LastName),
                    PhoneNumber = singleUser.PhoneNumber
                };
                model.AllUsersCollection.Add(modelUser);
            }
            return this.View(model) ;
        }

        [HttpGet]
        public IActionResult CompleteData()
        {
            var model = new CompleteUserDataModel();
            return this.View(model);
        }

        [HttpPost]
        public IActionResult CompleteData(CompleteUserDataModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = this.usersService.GetUserByName(this.User.Identity.Name);
            if (user ==null)
            {
                return this.NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.SecondContactNumber = model.SecondContactNumber;

            this.usersService.UpdateUserInfo(user);
            return this.Redirect("/");
        }


        public IActionResult Details(string fullName)
        {
            var user = this.usersService.GetUserByFullName(fullName);
            var model = new UserDetailViewModel
            {
                Id = user.Id,
                FullName = this.FullNameCreator(user.FirstName,user.LastName),               
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RegisteredOn = user.RegisteredOn.ToString("D"),
                SecondContactNumber = user.SecondContactNumber
            };
            return this.View(model);
        }

        [NonAction]
        private string FullNameCreator(string firstName, string lastName)
        {
            return firstName + ' ' + lastName;
        }
    }
}
