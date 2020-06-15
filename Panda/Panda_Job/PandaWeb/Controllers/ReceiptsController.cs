using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PandaWeb.Models.Receipt;

using Panda.Domain;
using Panda.Services;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System;

namespace Panda.App.Controllers
{
    [Authorize]
    public class ReceiptsController : Controller
    {
        private readonly IReceiptsService receiptsService;
        private readonly IPackagesService packagesService;
        private readonly IAddressesService addressesService;

        public ReceiptsController(IReceiptsService receiptsService, IPackagesService packagesService, IAddressesService addressesService)
        {            
            this.receiptsService = receiptsService;
            this.packagesService = packagesService;
            this.addressesService = addressesService;
        }
                
        public IActionResult Index()
        {
            List<ReceiptViewModelByUser> myReceiptsOfUser = this.receiptsService
                .GetAllReceiptsWithRecipient()
                .Select(receipt => new ReceiptViewModelByUser
                {
                    Id = receipt.Id,
                    Fee = receipt.Fee,
                    IssuedOn = receipt.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Recipient = receipt.Recipient.FirstName + receipt.Recipient.LastName.Substring(0,1)
                })
                .ToList();
            if (this.User.IsInRole("Admin"))
            {
                return View(myReceiptsOfUser);
            }
            else
            {
                var personalReceipt = myReceiptsOfUser.Where(r => r.Recipient == User.Identity.Name).ToList();
                return View(personalReceipt);
            }
        }

        [HttpGet("/Receipts/Details/{id}")]
        public IActionResult Details(string id)
        {
            Receipt receiptFromDb = this.receiptsService
                .GetAllReceiptsWithRecipientAndPackage()
                .Where(receipt => receipt.Id == id)                
                .SingleOrDefault();

            ReceiptDetailsViewModel viewModel = new ReceiptDetailsViewModel
            {
                Id = receiptFromDb.Id,
                Total = receiptFromDb.Fee,
                Recipient = receiptFromDb.Recipient.UserName,
                DeliveryAddress = this.addressesService
                            .ShortenedAddressToString(
                                     this.addressesService
                                     .GetAddressByIdAsync(receiptFromDb
                                                .Package.ShippingAddress).Result),
                PackageWeight = receiptFromDb.Package.Weight,
                PackageDescription = receiptFromDb.Package.Description,
                IssuedOn = receiptFromDb.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            };

            return this.View(viewModel);
        }

        [HttpGet("/Receipts/Create/{packageId}")]
        public IActionResult Create(string packageId)
        {
            var currentPackage = this.packagesService.GetPackageAsync(packageId).Result;
            var receipt = new Receipt
            {
                Fee = Convert.ToDecimal(currentPackage.Weight * 2.67),
                IssuedOn = DateTime.UtcNow,
                RecipientId = currentPackage.RecipientId,
                PackageId = currentPackage.Id
            };
            this.receiptsService.CreateReceiptAsync(receipt);

            TempData["RecieptsAdded"] = "Successfuly created a receipt";
            var id = receipt.Id;
            return this.Redirect($"/Receipts/Details/{id}");
        }
    }
}