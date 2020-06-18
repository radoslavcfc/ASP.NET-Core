using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Panda.Domain;
using Panda.Services;
using PandaWeb.Models.User;

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
        public async Task<IActionResult> Index()
        {
           var allUsersFromDb = await userManager
                .GetUsersInRoleAsync("user");

            if (allUsersFromDb == null)
            {
                return this.NotFound();
            }

            var model = new AllUsersIndexViewModel();

            foreach (var singleUser in allUsersFromDb)
            {
                var modelUser = new UserIndexViewModel
                {
                    Id = singleUser.Id,
                    FullName = this.FullNameCreator(singleUser.FirstName, singleUser.LastName),
                    PhoneNumber = singleUser.PhoneNumber,
                    IsDeleted = singleUser.IsDeleted
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
        [ValidateAntiForgeryToken]
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
            var selectedtUser = await this.usersService
                .GetUserByIdAsync(id);

            if (selectedtUser == null)
            {
                return this.NotFound();
            }

            var model = new UserDetailViewModel
            {
                Id = selectedtUser.Id,
                FullName = this.FullNameCreator(selectedtUser.FirstName, selectedtUser.LastName),
                Email = selectedtUser.Email,
                PhoneNumber = selectedtUser.PhoneNumber,
                RegisteredOn = selectedtUser.RegisteredOn.ToString("D"),
                SecondContactNumber = selectedtUser.SecondContactNumber
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

        /// <summary>
        /// Method verifying if the new entries from the user are different from the current ones
        /// </summary>
        /// <param name="email">New email ?</param>
        /// <param name="phoneNumber">New phone number ?</param>
        /// <param name="secondContactNumber">New second phone number</param>
        /// <returns>After successfull managing the new data, it redirects to the main manage panel</returns>
        /// 

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            //SecondContactNumber is not requiered if user wants to remove it then it will come as null

            currentUser.SecondContactNumber = 
                    (email == null && phoneNumber == null && secondContactNumber == null) ?
                     null : currentUser.SecondContactNumber;

            await this.userManager.UpdateAsync(currentUser);
            await this.userManager.UpdateNormalizedEmailAsync(currentUser);
            await this.usersService.SaveToDataBaseAsync();

            return this.Redirect("/Users/ManageData");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            var passwordModel = new ChangePasswordModel();
            return this.View(passwordModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ChangePassword")]
        public async Task<IActionResult> ChangePasswordPost(ChangePasswordModel model)
        {
            var currentUser = await this.GetTheCurrentUserAsync();
            if (currentUser == null)
            {
                return NotFound();
            }
           
            await this.userManager
                .ChangePasswordAsync(currentUser, model.CurrentPassword, model.NewPassword);   
            
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
        [ValidateAntiForgeryToken]
        [ActionName("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(DeleteAccountModel model)
        {
            var currentUser = await this.GetTheCurrentUserAsync();
            if (currentUser == null)
            {
                return NotFound();
            }
            var currentUserPass = await this.userManager.CheckPasswordAsync(currentUser, model.Password);
            if (currentUserPass && model.IAgree)
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HardDelete(string id)
        {
            var userFromDb = await this.usersService
                .GetUserByIdWithDeletedAsync(id);

            if (userFromDb == null)
            {
                return this.NotFound();
            }

            var model = new HardDeleteUserModel
            {
                Id = userFromDb.Id,
                FullName = this.FullNameCreator(userFromDb.FirstName, userFromDb.LastName)
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> HardDelete(HardDeleteUserModel model)
        {
            if (!ModelState.IsValid || model.Confirm == false)
            {
                return this.View(model);
            }

            var idOfUser = model.Id;

            var user = await this.usersService
                .GetUserByIdWithDeletedAsync(idOfUser);

            if (user == null)
            {
                return this.NotFound();
            }
            await this.usersService.DeleteAllDataForUserAsync(idOfUser);
            await this.userManager.DeleteAsync(user);
            TempData["UserRemoved"] = "The user has been removed from the database!";
            return this.RedirectToAction("Index", "Users");
        }


        //Help methods

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
