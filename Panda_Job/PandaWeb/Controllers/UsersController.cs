using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Panda.Domain;
using Panda.Services;
using PandaWeb.Models.User;
using System.Threading.Tasks;

namespace PandaWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        
        private readonly UserManager<PandaUser> userManager;

        public UsersController(IUsersService usersService, UserManager<PandaUser> userManager)
        {
            this.usersService = usersService;
            this.userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var allUsersFromDb = userManager.GetUsersInRoleAsync("user").Result;
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
            return this.View(model);
        }

        [HttpGet]
        public IActionResult CompleteData()
        {
            var model = new UserCompleteDataModel();
            return this.View(model);
        }

        [HttpPost]
        public IActionResult CompleteData(UserCompleteDataModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = this.userManager.GetUserAsync(this.User).Result;
            if (user == null)
            {
                return this.NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.SecondContactNumber = model.SecondContactNumber;
            this.userManager.UpdateAsync(user);
            
            return this.Redirect("/");
        }


        public IActionResult Details(string fullName)
        {
           
            var user = this.usersService.GetUserByFullName(fullName);
            var model = new UserDetailViewModel
            {
                Id = user.Id,
                FullName = this.FullNameCreator(user.FirstName, user.LastName),
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RegisteredOn = user.RegisteredOn.ToString("D"),
                SecondContactNumber = user.SecondContactNumber
            };
            return this.View(model);
        }

        [HttpGet("Users/ShowData")]
        public IActionResult ShowData()
        {
            var currentUser = this.userManager.GetUserAsync(this.User).Result;

            ShowUsersDataModel model = new ShowUsersDataModel
            {
                Id = currentUser.Id,
                UserName = currentUser.UserName,
                RegisteredOn = currentUser.RegisteredOn.ToString("dd/MM/yyyy"),
                PhoneNumber = currentUser.PhoneNumber,
                SecondContactNumber = currentUser.SecondContactNumber,
                Email = currentUser.Email,
                FullName = this.FullNameCreator(currentUser.FirstName, currentUser.LastName)
            };

            return this.View(model);
        }

        public async Task<IActionResult> EditInfo(string email, string phoneNumber, string secondContactNumber)
        {
            var currentUser = this.userManager.GetUserAsync(this.User).Result;
            if (currentUser == null)
            {
                return NotFound();
            }
            
            //if modify one field the others will come as null parameters
            currentUser.Email = email?? currentUser.Email;
            currentUser.PhoneNumber = phoneNumber ?? currentUser.PhoneNumber;
            currentUser.SecondContactNumber = secondContactNumber ?? currentUser.SecondContactNumber;

            //SecondContactNumber is not requiered if user wants to remove it then it will be null

            currentUser.SecondContactNumber = 
                    (email == null && phoneNumber == null && secondContactNumber == null) ? null : currentUser.SecondContactNumber;

            await this.userManager.UpdateAsync(currentUser);
            await this.userManager.UpdateNormalizedEmailAsync(currentUser);
            this.usersService.SaveToDataBaseAsync();
            return this.Redirect("/Users/ShowData");
        }

        public async Task<IActionResult> EditEditPhoneNumber(string email)
        {
            var currentUser = this.userManager.GetUserAsync(this.User).Result;
            if (currentUser == null)
            {
                return NotFound();
            }
            currentUser.Email = email;
            await this.userManager.UpdateAsync(currentUser);
            await this.userManager.UpdateNormalizedEmailAsync(currentUser);
            this.usersService.SaveToDataBaseAsync();
            return this.Redirect("/Users/ShowData");
        }
        [NonAction]
        private string FullNameCreator(string firstName, string lastName)
        {
            return firstName + ' ' + lastName;
        }
    }
}
