﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Panda.Data;
using Panda.Domain;
using Panda.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public class AddressesService : IAddressesService
    {
        //Short address string symbols:
        private const string spacePlusComa = ", ";
        private const string appartment = "Ap. ";
        private const string floor = "Floor ";
        private const string entrance = "Ent. ";

        private readonly PandaDbContext pandaDbContext;
       
        public AddressesService(PandaDbContext pandaDbContext)
        {
            this.pandaDbContext = pandaDbContext;
        }

        public int CountOfAddressesPerUser(PandaUser user)
        {
            var count = pandaDbContext
                .Addresses
                .Where(a => a.IsDeleted == false)
                .Count(a => a.UserId == user.Id);

            return count;
        }

        public void CreateAddress(Address address)
        {
            this.pandaDbContext.Addresses.Add(address);
            this.pandaDbContext.SaveChanges();
        }

        public Address GetAddressById(string addressId)
        {
            var addresFromDb = this.pandaDbContext
                .Addresses
                .Where(a => a.Id == addressId && a.IsDeleted == false)
                .Include(a => a.Flat)
                .FirstOrDefault();
            return addresFromDb;
        }

        public IQueryable<Address> ListOfAddressesByUser(string userName)
        {
            var user = this.pandaDbContext
                .Users
                .Where(u => u.UserName == userName);

            var list = this.pandaDbContext
                .Addresses
                .Where(a => a.UserId == user.FirstOrDefault().Id && a.IsDeleted == false)
                .OrderBy(a => a.AddressType)
                .Include(a => a.Flat);
                
            return list;
        }

        public string ShortenedAddressToString(Address fullAddress)
        {
            var addressToString = fullAddress.Country.Substring(0, 3).ToUpper() +
                spacePlusComa +
                fullAddress.Region.Substring(0, 3).ToUpper() +
                spacePlusComa +
                fullAddress.Town.ToUpper() +
                spacePlusComa +
                fullAddress.StreetName +
                spacePlusComa +
                fullAddress.Number;
            if (fullAddress.PropertyType == PropertyType.Flat)
            {
                addressToString +=
                    spacePlusComa +
                    entrance +
                    fullAddress.Flat.Entrance +
                    spacePlusComa +
                     appartment +
                     fullAddress.Flat.Apartment +
                     spacePlusComa +
                     floor +
                     fullAddress.Flat.Floor;    
            }
            return addressToString;
        }

        public  void MarkAsDeleted(string id)
        {
            var addres = this.pandaDbContext.Addresses
                .Where(a => a.Id == id)
                .FirstOrDefault();
             addres.IsDeleted = true;
             pandaDbContext.SaveChanges();
        }
    }
}
