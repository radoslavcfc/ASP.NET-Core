using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PandaWeb.Models.Package;

using Panda.Domain;
using Panda.Domain.Enums;
using Panda.Services;
using System;
using System.Globalization;
using System.Linq;

namespace Panda.App.Controllers
{
    [Authorize]
    public class PackagesController : Controller
    {
        private readonly UserManager<PandaUser> userManager;
        private readonly IUsersService usersService;
        private readonly IPackagesService packagesService;
        private readonly IReceiptsService receiptsService;

        public PackagesController(UserManager<PandaUser> userManager, IUsersService usersService, IPackagesService packagesService, IReceiptsService receiptsService)
        {
            this.userManager = userManager;
            this.usersService = usersService;
            this.packagesService = packagesService;
            this.receiptsService = receiptsService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var viewModel = new PackageCreateBindingModel();
          
            viewModel.UsersCollection = this.usersService.GetAllUsers().Select(u => new UserDropDownModel
            {
                Id = u.Id,
                Name = u.UserName
            });
            
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(PackageCreateBindingModel bindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Packages/Create");
            }

            Package package = new Package
            {
                Description = bindingModel.Description,
                RecipientId = this.usersService.GetUserById(bindingModel.Recipient).Id,
                ShippingAddress = bindingModel.ShippingAddress,
                Weight = bindingModel.Weight,
                Status = PackageStatus.Pending,
                EstimatedDeliveryDate = DateTime.UtcNow.AddDays(4)
            };

            this.packagesService.CreatePackage(package);
            TempData["SuccessCreate"] = "Well Done Mate!";
            return this.Redirect($"/Packages/Details/{package.Id}");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var collection = this.packagesService.GetAllPackages()                
                .Select(p => new PackageHomeViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Status = p.Status.ToString(),
                    UserId = p.RecipientId
                })
                .ToList();

            if (this.User.IsInRole("Admin"))
            {
                return this.View(collection);
            }
            else
            {
                var personalColl = collection
                     .Where(p => p.UserId == this.userManager.GetUserId(this.User))
                     .ToList();
                return this.View(personalColl);
            }
        }

        [HttpGet]
        public IActionResult Pending()
        {
            var model = this.packagesService.GetAllPackages()
                .Where(p => p.Status == PackageStatus.Pending)
                .Select(p => new PackagePendingViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Weight = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    Recipient = this.usersService.GetUserById(p.RecipientId).UserName
                }).ToList();
            if (this.User.IsInRole("Admin"))
            {
                return this.View(model);
            }
            else
            {
                var personalCol = model.Where(p => p.Recipient == this.User.Identity.Name).ToList();
                return this.View(personalCol);
            }           
        }

        [HttpGet]
        public IActionResult Deliver(string Id)
        {
            var currentPackage = this.packagesService.GetPackage(Id);
            currentPackage.Status = PackageStatus.Delivered;
            this.packagesService.UpdatePackage(currentPackage);
            var receipt = new Receipt
            {
                Fee = Convert.ToDecimal(currentPackage.Weight * 2.67),
                IssuedOn = DateTime.UtcNow,
                RecipientId = currentPackage.RecipientId,
                PackageId = currentPackage.Id
            };
            this.receiptsService.CreateReceipt(receipt);
            
            return this.Redirect("/Receipts/Index");
        }

        [HttpGet]
        public IActionResult Delivered()
        {
            var model = this.packagesService.GetAllPackages()
                .Where(p => p.Status == PackageStatus.Delivered)
                .Select(p => new PackageDeliveredViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Weight = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    Recipient = this.usersService.GetUserById(p.RecipientId).UserName
                }).ToList();

            if (this.User.IsInRole("Admin"))
            {
                return this.View(model);
            }
            else
            {
                var personalCol = model.Where(p => p.Recipient == this.User.Identity.Name).ToList();
                return this.View(personalCol);
            }
        }

        [HttpGet]
        public IActionResult Details(string Id)
        {
            var package = this.packagesService.GetPackage(Id);
            var model = new PackageDetailsViewModel
            {
                ShippingAddress = package.ShippingAddress,
                Status = package.Status.ToString(),
                EstimatedDeliveryDate = package.EstimatedDeliveryDate?
                    .ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Weight = package.Weight,
                Recipient = package.Recipient.UserName,
                Description = package.Description
            };
            return this.View(model);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return this.Ok("HAHA");
        }
    }
}