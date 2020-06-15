using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Panda.Domain;
using Panda.Services;
using PandaWeb.Models.User;
using System;
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
            this.usersService.UpdateUserInfo(user);
            return this.Redirect("/");
        }


        public IActionResult Details(string id)
        {
            var user = this.usersService.GetUserById(id).Result;
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

        [HttpGet("Users/ManageData")]
        public IActionResult ManageData()
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
            await this.usersService.SaveToDataBaseAsync();
            return this.Redirect("/Users/ShowData");
        }

        [HttpGet]

       
        public IActionResult  ChangePassword()
        {
           var currentUser = userManager.GetUserAsync(this.User).Result;
            if (currentUser == null)
            {
                return NotFound();
            }
            var passwordModel = new ChangePasswordModel();
            return this.View(passwordModel);
        }

        [HttpPost]
        [ActionName("ChangePassword")]
        public async Task<IActionResult> ChangePasswordPost(ChangePasswordModel model)
        {
            var currentUser = await userManager.GetUserAsync(this.User);
            if (currentUser == null)
            {
                return NotFound();
            }
           
            await this.userManager.ChangePasswordAsync(currentUser, model.CurrentPassword, model.NewPassword);            
            await this.signInManager.SignOutAsync();
            TempData["Changed Password"] = "Your password was successfully changed, Please sign in ";
            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteAccount()
        {
            var currentUser = userManager.GetUserAsync(this.User).Result;
            if (currentUser == null)
            {
                return NotFound();
            }
            var passwordModel = new DeleteAccountModel();
            return this.View(passwordModel);
        }

        [HttpPost]
        [ActionName("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(DeleteAccountModel model)
        {
            var currentUser = await userManager.GetUserAsync(this.User);
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
        private string FullNameCreator(string firstName, string lastName)
        {
            return firstName + ' ' + lastName;
        }
    }
}
