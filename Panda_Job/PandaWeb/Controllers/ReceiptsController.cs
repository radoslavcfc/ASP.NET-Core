using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PandaWeb.Models.Receipt;

using Panda.Domain;
using Panda.Services;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Panda.App.Controllers
{
    [Authorize]
    public class ReceiptsController : Controller
    {
        private readonly IReceiptsService receiptsService;        

        public ReceiptsController(IReceiptsService receiptsService)
        {            
            this.receiptsService = receiptsService;
        }
                
        public IActionResult Index()
        {
            List<ReceiptViewModelByUser> myReceiptsOfUser = this.receiptsService.GetAllReceiptsWithRecipient()
                .Select(receipt => new ReceiptViewModelByUser
                {
                    Id = receipt.Id,
                    Fee = receipt.Fee,
                    IssuedOn = receipt.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Recipient = receipt.Recipient.UserName
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
            Receipt receiptFromDb = this.receiptsService.GetAllReceiptsWithRecipientAndPackage()
                .Where(receipt => receipt.Id == id)                
                .SingleOrDefault();

            ReceiptDetailsViewModel viewModel = new ReceiptDetailsViewModel
            {
                Id = receiptFromDb.Id,
                Total = receiptFromDb.Fee,
                Recipient = receiptFromDb.Recipient.UserName,
                DeliveryAddress = receiptFromDb.Package.ShippingAddress,
                PackageWeight = receiptFromDb.Package.Weight,
                PackageDescription = receiptFromDb.Package.Description,
                IssuedOn = receiptFromDb.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            };

            return this.View(viewModel);
        }
    }
}