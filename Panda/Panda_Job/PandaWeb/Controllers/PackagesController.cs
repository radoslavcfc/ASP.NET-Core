﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
//using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using PandaWeb.Models.Package;
using PandaWeb.Models.User;
using Panda.Domain;
using Panda.Domain.Enums;
using Panda.Services;
using Microsoft.Extensions.Logging;

namespace Panda.App.Controllers
{
    [Authorize]
    public class PackagesController : Controller
    {
        private readonly UserManager<PandaUser> _userManager;
        private readonly IUsersService _usersService;
        private readonly IPackagesService _packagesService;
        private readonly IAddressesService _addressesService;
        private readonly ILogger<PackagesController> _logger;

        public PackagesController(
            UserManager<PandaUser> userManager,
            IUsersService usersService,
            IPackagesService packagesService,
            IAddressesService addressesService,
            ILogger<PackagesController> logger)
        {
            this._userManager = userManager;
            this._usersService = usersService;
            this._packagesService = packagesService;
            this._addressesService = addressesService;
            this._logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var collectionFromDb = this._packagesService.GetAllPackages();

            if (collectionFromDb == null)
            {
                _logger.LogWarning($"Collection with packages from DB - NOT FOUND");
                return this.NotFound();
            }

            var modelCollection = collectionFromDb
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
                return this.View(modelCollection);
            }
            else
            {
                var personalColl = modelCollection
                     .Where(p => p.UserId == this._userManager.GetUserId(this.User))
                     .ToList();

                return this.View(personalColl);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var viewModel = new PackageCreateModel();

            var usersCollection = this._usersService.GetAllUsersNoAdmins();

            if (usersCollection == null)
            {
                _logger.LogWarning($"Collection with users from DB - NOT FOUND");
                return this.NotFound();
            }

            viewModel.UsersCollection =
               usersCollection
               .Select(u => new UserDropDownModel
               {
                   Id = u.Id,
                   Name = u.UserName
               });

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PackageCreateModel bindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                _logger.LogWarning($"State of binding model PackageCreateModel - INVALID!");
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

            await this._packagesService
                .CreatePackageAsync(package);

            _logger.LogInformation($"Package with id {package.Id} - successfully created");

            TempData["SuccessCreatedPackage"] = "A New package has been successfuly created!";
            return this.Redirect($"/Packages/Details/{package.Id}");
        }

        [HttpGet("/Packages/ListAll/{status}")]
        public async Task<IActionResult> ListAll(string status)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                _logger.LogWarning($"User with ID {user.Id} - NOT FOUND");
                return this.NotFound();
            }

            var currentUserRole = await this._usersService.GetRoleByIdAsync(user.Id);

            if (currentUserRole == null)
            {
                _logger.LogWarning($"Role for user with {user.Id} - NOT FOUND");
                return this.NotFound();
            }

            IEnumerable<Package> packageFromDb;

            if (currentUserRole == "admin")
            {
                packageFromDb = this._packagesService
                    .GetAllPackagesWithStatusForAdmin(status);
            }

            else
            {
                //var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                packageFromDb = this._packagesService
                    .GetAllPackagesWithStatusForUser(user.Id, status);
            }

            if (packageFromDb == null)
            {
                _logger.LogWarning($"Package for user with id {user.Id} - NOT FOUND");
                return this.NotFound();
            }

            var model = packageFromDb
                .Select(p => new PackageListAllViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Weight = p.Weight,
                    ShippingAddress = this._addressesService.ShortenedAddressToString(
                         this._addressesService.GetAddressByIdAsync(p.ShippingAddress).Result),
                    RecipientFullName =
                        (this._usersService.GetUserByIdWithDeletedAsync(p.RecipientId).Result.FirstName + " " +
                        (this._usersService.GetUserByIdWithDeletedAsync(p.RecipientId).Result.LastName).Substring(0, 1)),
                    RecipientId = p.RecipientId

                }).ToList();

            ViewData["Status"] = status.ToLower();

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string Id)
        {
            var package = await this._packagesService
                .GetPackageAsync(Id);

            if (package == null)
            {
                _logger.LogWarning($"Package with id {Id} - NOT FOUND!");
                return this.NotFound();
            }

            var model = new PackageDetailsViewModel
            {
                Id = package.Id,
                //
                //!!!To Optimzie!!!
                ShippingAddress = this._addressesService
                    .ShortenedAddressToString(
                        await this._addressesService.GetAddressByIdAsync(package.ShippingAddress)),

                /////////////////////////////////////////////////////////////////////////////////////
                Status = package.Status.ToString(),
                EstimatedDeliveryDate = package.EstimatedDeliveryDate?
                    .ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Weight = package.Weight,
                RecipientFullName = package.Recipient.FirstName + " " + package.Recipient.LastName.Substring(0, 1),
                Description = package.Description
            };

            return this.View(model);
        }

        [HttpGet("/Packages/Deliver/{packageId}")]

        public async Task<IActionResult> Deliver(string packageId)
        {
            var currentPackage = await this._packagesService
                .GetPackageAsync(packageId);

            if (currentPackage == null)
            {
                _logger.LogWarning($"Package with id - {packageId} - NOT FOUND");
                return this.NotFound();
            }

            currentPackage.Status = PackageStatus.Delivered;
            await this._packagesService.UpdatePackageAsync(currentPackage);

            _logger.LogInformation($"Package with id {packageId} has been successfully updated");

            return this.Redirect($"/Receipts/Create/{packageId}");
        }
    }
}