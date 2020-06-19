using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PandaWeb.Models.Receipt;
using Panda.Domain;
using Panda.Services;

namespace Panda.App.Controllers
{
    [Authorize]
    public class ReceiptsController : Controller
    {
        private readonly IReceiptsService _receiptsService;
        private readonly IPackagesService _packagesService;
        private readonly IAddressesService _addressesService;

        public ReceiptsController(IReceiptsService receiptsService, 
            IPackagesService packagesService, IAddressesService addressesService)
        {            
            this._receiptsService = receiptsService;
            this._packagesService = packagesService;
            this._addressesService = addressesService;
        }
                
        public IActionResult Index()
        {
            var collectionFromDb = this._receiptsService
                .GetAllReceiptsWithRecipient();

            if (collectionFromDb == null)
            {
                return this.NotFound();
            }

            List<ReceiptViewModelByUser> modelCollection = collectionFromDb
                .Select(receipt => new ReceiptViewModelByUser
                {
                    Id = receipt.Id,
                    Fee = receipt.Fee,
                    IssuedOn = receipt.IssuedOn
                                .ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Recipient = receipt.Recipient
                                .FirstName + receipt.Recipient.LastName.Substring(0,1)
                })
                .ToList();

            if (this.User.IsInRole("Admin"))
            {
                return View(modelCollection);
            }

            else
            {
                var personalReceipt = modelCollection
                    .Where(r => r.Recipient == User.Identity.Name)
                    .ToList();
                return View(personalReceipt);
            }
        }

        [HttpGet("/Receipts/Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            Receipt receiptFromDb = this._receiptsService
                .GetAllReceiptsWithRecipientAndPackage()
                .Where(receipt => receipt.Id == id)                
                .SingleOrDefault();

            if (receiptFromDb == null)
            {
                return this.NotFound();
            }

            ReceiptDetailsViewModel viewModel = new ReceiptDetailsViewModel
            {
                Id = receiptFromDb.Id,
                Total = receiptFromDb.Fee,
                Recipient = receiptFromDb.Recipient.UserName,

                //TODO - simplify 
                DeliveryAddress = this._addressesService
                            .ShortenedAddressToString(
                                     await this._addressesService
                                     .GetAddressByIdAsync(receiptFromDb
                                                .Package.ShippingAddress)),
                /////////////////////////////////////////////////////////////
                PackageWeight = receiptFromDb.Package.Weight,
                PackageDescription = receiptFromDb.Package.Description,
                IssuedOn = receiptFromDb.IssuedOn
                    .ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            };

            return this.View(viewModel);
        }

        [HttpGet("/Receipts/Create/{packageId}")]
        public async Task<IActionResult> Create(string packageId)
        {
            var currentPackage = await this._packagesService
                .GetPackageAsync(packageId);

            if (currentPackage == null)
            {
                return this.NotFound();
            }

            var receipt = new Receipt
            {
                Fee = Convert.ToDecimal(currentPackage.Weight * 2.67),
                IssuedOn = DateTime.UtcNow,
                RecipientId = currentPackage.RecipientId,
                PackageId = currentPackage.Id
            };

            await this._receiptsService.CreateReceiptAsync(receipt);

            TempData["RecieptsAdded"] = "Successfuly created a receipt";
            var id = receipt.Id;
            return this.Redirect($"/Receipts/Details/{id}");
        }
    }
}