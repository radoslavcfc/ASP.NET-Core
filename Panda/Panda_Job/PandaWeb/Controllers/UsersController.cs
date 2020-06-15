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
        private readonly SignInManager<PandaUser> signInManager;

        public UsersController(IUsersService usersService, UserManager<PandaUser> userManager,
            SignInManager<PandaUser> signInManager)
        {
            this.usersService = usersService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var allUsersFromDb = usersService.GetAllUsersNoAdmins();
            var model = new AllUsersIndexViewModel();
            foreach (var singleUser in allUsersFromDb)
            {
                var modelUser = new UserIndexViewModel
                {
                    Id = singleUser.Id,
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
        public async Task<IActionResult> CompleteData(UserCompleteDataModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var currentUser = await GetTheCurrentUserAsync();
            if (currentUser == null)
            {
                return this.NotFound();
            }

            currentUser.FirstName = model.FirstName;
            currentUser.LastName = model.LastName;
            currentUser.PhoneNumber = model.PhoneNumber;
            currentUser.SecondContactNumber = model.SecondContactNumber;

            await this.userManager.UpdateAsync(currentUser);
            await this.usersService.UpdateUserInfoAsync(currentUser);
            return this.Redirect("/");
        }

        public async Task<IActionResult> Details(string id)
        {
            var currentUser = await this.GetTheCurrentUserAsync();
            var model = new UserDetailViewModel
            {
                Id = currentUser.Id,
                FullName = this.FullNameCreator(currentUser.FirstName, currentUser.LastName),
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                RegisteredOn = currentUser.RegisteredOn.ToString("D"),
                SecondContactNumber = currentUser.SecondContactNumber
            };
            return this.View(model);
        }

        [HttpGet("Users/ManageData")]
        public async Task<IActionResult> ManageData()
        {
            var currentUser = await this.GetTheCurrentUserAsync();

            if (currentUser == null)
            {
                return NotFound();
            }

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
            var currentUser = await this.GetTheCurrentUserAsync();
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
            await this.usersService.SaveToDataBaseAsync();
            return this.Redirect("/Users/ShowData");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            var passwordModel = new ChangePasswordModel();
            return this.View(passwordModel);
        }

        [HttpPost]
        [ActionName("ChangePassword")]
        public async Task<IActionResult> ChangePasswordPost(ChangePasswordModel model)
        {
            var currentUser = await this.GetTheCurrentUserAsync();
            if (currentUser == null)
            {
                return NotFound();
            }
           
            await this.userManager.ChangePasswordAsync(currentUser, model.CurrentPassword, model.NewPassword);            
            await this.signInManager.SignOutAsync();
            TempData["Changed Password"] = "Your password was successfully changed, Please sign in ";
            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult DeleteAccount()
        {
            var deleteModel = new DeleteAccountModel();
            return this.View(deleteModel);
        }

        [HttpPost]
        [ActionName("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(DeleteAccountModel model)
        {
            var currentUser = await this.GetTheCurrentUserAsync();
            if (currentUser == null)
            {
                return NotFound();
            }
            var currentUserPass = await this.userManager.CheckPasswordAsync(currentUser, model.Password);
            if (currentUserPass)
            {
                await this.usersService.DeleteAccountAsync(currentUser);
                await this.signInManager.SignOutAsync();
                
                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                return this.View(model);
            }
        }

        [NonAction]
        private async Task<PandaUser> GetTheCurrentUserAsync()
        {
            var currentUser = await userManager.GetUserAsync(this.User);
            return currentUser;
        }
        [NonAction]
        private string FullNameCreator(string firstName, string lastName)
        {
            return firstName + ' ' + lastName;
        }
    }
}
