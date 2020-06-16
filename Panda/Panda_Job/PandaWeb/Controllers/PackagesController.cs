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
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

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

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var viewModel = new PackageCreateModel();

            viewModel.UsersCollection =
               this.usersService.GetAllUsersNoAdmins()
               .Select(u => new UserDropDownModel
               {
                   Id = u.Id,
                   Name = u.UserName
               });
            
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PackageCreateModel bindingModel)
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
                CreatedOn = DateTime.UtcNow,
                EstimatedDeliveryDate = DateTime.UtcNow.AddDays(4)
            };

            await this.packagesService.CreatePackageAsync(package);
            TempData["SuccessCreatedPackage"] = "A New package has been successfuly created!";
            return this.Redirect($"/Packages/Details/{package.Id}");
        }

        [HttpGet ("/Packages/ListAll/{status}")]
        public IActionResult ListAll(string status)
        {
            var currentUserRole = this.User.Identity.Name;
                
            IEnumerable<Package> packageFromDb;

            if (currentUserRole == "admin")
            {
                packageFromDb = this.packagesService
                    .GetAllPackagesWithStatusForAdmin(status);
            }

            else 
            {
                var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                packageFromDb = this.packagesService
                    .GetAllPackagesWithStatusForUser(currentUserId, status); 
            }

            var model = packageFromDb
                .Select(p => new PackageListAllViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Weight = p.Weight,
                    Status = p.Status.ToString(),
                    ShippingAddress = this.addressesService.ShortenedAddressToString(
                         this.addressesService.GetAddressByIdAsync(p.ShippingAddress).Result),
                    RecipientFullName =
                        (this.usersService.GetUserByIdWithDeletedAsync(p.RecipientId).Result.FirstName + " " +
                        (this.usersService.GetUserByIdWithDeletedAsync(p.RecipientId).Result.LastName).Substring(0, 1)),
                    RecipientId = p.RecipientId

                }).ToList();

            return this.View(model);                     
        }

        //[HttpGet]
        //public IActionResult Delivered()
        //{
        //    var model = this.packagesService.GetAllPackages()
        //        .Where(p => p.Status == PackageStatus.Delivered)
        //        .Select(p => new PackageListAllViewModel
        //        {
        //            Id = p.Id,
        //            Description = p.Description,
        //            Weight = p.Weight,
        //            ShippingAddress = this.addressesService
        //            .ShortenedAddressToString(this.addressesService.GetAddressByIdAsync
        //            (p.ShippingAddress).Result),
        //            RecipientFullName =
        //                (this.usersService.GetUserByIdAsync(p.RecipientId).Result.FirstName + " " +
        //                (this.usersService.GetUserByIdAsync(p.RecipientId).Result.LastName).Substring(0, 1)),
        //            RecipientId = p.RecipientId
        //        }).ToList();

        //    if (this.User.IsInRole("Admin"))
        //    {
        //        return this.View(model);
        //    }
        //    else
        //    {
        //        var personalCol = model
        //            .Where(p => 
        //                p.RecipientId == this.userManager
        //                    //.GetUserId(this.User)
        //                    .GetUserAsync(this.User)
        //                    .Result.Id)
        //            .ToList();
        //        return this.View(personalCol);
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> Details(string Id)
        {
            var package = await this.packagesService
                .GetPackageAsync(Id);

            var model = new PackageDetailsViewModel
            {
                Id = package.Id,
                ShippingAddress = this.addressesService
                    .ShortenedAddressToString(
                        await this.addressesService.GetAddressByIdAsync(package.ShippingAddress)),
                Status = package.Status.ToString(),
                EstimatedDeliveryDate = package.EstimatedDeliveryDate?
                    .ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Weight = package.Weight,
                RecipientFullName = package.Recipient.FirstName + " " + package.Recipient.LastName.Substring(0,1),
                Description = package.Description
            };

            return this.View(model);
        }

        [HttpGet("/Packages/Deliver/{packageId}")]

        public async Task<IActionResult> Deliver(string packageId)
        {
            var currentPackage = await this.packagesService
                .GetPackageAsync(packageId);

            if (currentPackage == null)
            {
                return this.NotFound();
            }

            currentPackage.Status = PackageStatus.Delivered;
            await  this.packagesService.UpdatePackageAsync(currentPackage);

            return this.Redirect($"/Receipts/Create/{packageId}");
        }
    }
}