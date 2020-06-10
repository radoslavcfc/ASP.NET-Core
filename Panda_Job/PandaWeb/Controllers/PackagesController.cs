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
using PandaWeb.Models.User;

namespace Panda.App.Controllers
{
    [Authorize]
    public class PackagesController : Controller
    {
        private readonly UserManager<PandaUser> userManager;
        private readonly IUsersService usersService;
        private readonly IPackagesService packagesService;
    
        private readonly IAddressesService addressesService;

        public PackagesController(
            UserManager<PandaUser> userManager,
            IUsersService usersService, 
            IPackagesService packagesService,
           
            IAddressesService addressesService)
        {
            this.userManager = userManager;
           this.usersService = usersService;
            this.packagesService = packagesService;
           
            this.addressesService = addressesService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var viewModel = new PackageCreateBindingModel();
          
            viewModel.UsersCollection = this.userManager
                .GetUsersInRoleAsync("admin")
                .Result.Select(u => new UserDropDownModel
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
                RecipientId = bindingModel.Recipient,
                ShippingAddress = bindingModel.ShippingAddress,
                Weight = bindingModel.Weight,
                Status = PackageStatus.Pending,
                EstimatedDeliveryDate = DateTime.UtcNow.AddDays(4)
            };

            this.packagesService.CreatePackage(package);
            TempData["SuccessCreatedPackage"] = "A New package has been successfuly created!";
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
                    ShippingAddress = this.addressesService
                        .ShortenedAddressToString(
                                this.addressesService
                                    .GetAddressById(
                                        p.ShippingAddress)),
                    RecipientFullName = 
                        (this.usersService.GetUserById(p.RecipientId).FirstName + " " +
                        (this.usersService.GetUserById(p.RecipientId).LastName).Substring(0,1)),
                    RecipientId = p.RecipientId

                }).ToList();
            if (this.User.IsInRole("Admin"))
            {
                return this.View(model);
            }
            else
            {
               
                var personalCol = model.Where(p => p.RecipientId == this.userManager.GetUserId(this.User)).ToList();
                return this.View(personalCol);
            }           
        }

        [HttpGet("/Packages/Deliver/{packageId}")]

        public IActionResult Deliver(string packageId)
        {
            var currentPackage = this.packagesService.GetPackage(packageId);
            currentPackage.Status = PackageStatus.Delivered;
            this.packagesService.UpdatePackage(currentPackage);
            
            return this.Redirect($"/Receipts/Create/{packageId}");
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
                    ShippingAddress = this.addressesService
                    .ShortenedAddressToString(this.addressesService.GetAddressById
                    (p.ShippingAddress)),
                    RecipientFullName =
                        (this.usersService.GetUserById(p.RecipientId).FirstName + " " +
                        (this.usersService.GetUserById(p.RecipientId).LastName).Substring(0, 1)),
                    RecipientId = p.RecipientId
                }).ToList();

            if (this.User.IsInRole("Admin"))
            {
                return this.View(model);
            }
            else
            {
                var personalCol = model
                    .Where(p => 
                        p.RecipientId == this.userManager
                            //.GetUserId(this.User)
                            .GetUserAsync(this.User)
                            .Result.Id)
                    .ToList();
                return this.View(personalCol);
            }
        }

        [HttpGet]
        public IActionResult Details(string Id)
        {
            var package = this.packagesService.GetPackage(Id);
            var model = new PackageDetailsViewModel
            {
                Id = package.Id,
                ShippingAddress = this.addressesService
                    .ShortenedAddressToString(this.addressesService.GetAddressById
                    (package.ShippingAddress)),
                Status = package.Status.ToString(),
                EstimatedDeliveryDate = package.EstimatedDeliveryDate?
                    .ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Weight = package.Weight,
                RecipientFullName = package.Recipient.FirstName + " " + package.Recipient.LastName.Substring(0,1),
                Description = package.Description
            };
            return this.View(model);
        }

    }
}