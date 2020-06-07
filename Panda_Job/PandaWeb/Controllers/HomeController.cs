﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panda.Services;
using PandaWeb.Models.Address;
using PandaWeb.Models.Error;
using System;
using System.Diagnostics;

namespace Panda.App.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        private readonly IUsersService usersService;
        public HomeController(IUsersService usersService, IAddressesService addressesService)
        {
            this.usersService = usersService;
            AddressesService = addressesService;
        }
        public IAddressesService AddressesService { get; }

        public IActionResult Index()
        {
            throw new Exception();
            if (this.User.Identity.IsAuthenticated)
            {
                var currentUserName = this.User.Identity.Name;
                var currentUser = this.usersService.GetUserByUserName(currentUserName);
                var currentUserAddressesCount = this.AddressesService.CountOfAddressesPerUser(currentUser);
                if (currentUserAddressesCount == 0)
                {
                    var addressModel = new AddNewAddressModel();
                    return this.View(addressModel);
                }
            }
            return this.View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult StatusCode(string code)
        {
            var model = new StatusCodeModel();
            model.ErrorStatusCode = code;
            return this.View(model);
        }
    }
}
