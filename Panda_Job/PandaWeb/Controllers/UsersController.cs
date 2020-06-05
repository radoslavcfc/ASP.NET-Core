﻿using Microsoft.AspNetCore.Mvc;
using Panda.Domain;
using Panda.Services;
using PandaWeb.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PandaWeb.Controllers
{

    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IAddressesService addressesService;

        public UsersController(IUsersService usersService, IAddressesService addressesService)
        {
            this.usersService = usersService;
            this.addressesService = addressesService;
        }
        public IActionResult Index()
        {
            var allUsersFromDb = usersService.GetAllUsers();
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
            return this.View(model) ;
        }

        public IActionResult Details(string fullName)
        {
            var user = this.usersService.GetUserByFullName(fullName);
            var model = new UserDetailViewModel
            {
                Id = user.Id,
                FullName = this.FullNameCreator(user.FirstName,user.LastName),               
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RegisteredOn = user.RegisteredOn.ToString("D"),
                SecondContactNumber = user.SecondContactNumber
            };
            return this.View(model);
        }

        [NonAction]
        private string FullNameCreator(string firstName, string lastName)
        {
            return firstName + ' ' + lastName;
        }
    }
}
