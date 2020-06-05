using Microsoft.EntityFrameworkCore;
using Panda.Data;
using Panda.Data.Migrations;
using Panda.Domain;
using Panda.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Panda.Services
{
    public class AddressesService : IAddressesService
    {
        private readonly PandaDbContext pandaDbContext;
        private const string spacePlusComa = ", ";
        private const string appartment = "Ap. ";
        private const string floor = "Floor ";
        private const string entrance = "Ent. ";
        public AddressesService(PandaDbContext pandaDbContext)
        {
            this.pandaDbContext = pandaDbContext;
        }

        public int CountOfAddressesPerUser(PandaUser user)
        {
            var count = pandaDbContext.Addresses.Count(a => a.UserId == user.Id);
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
                .Where(a => a.Id == addressId)
               .Include(a => a.Flat)
                .FirstOrDefault();
            return addresFromDb;
        }

        public IEnumerable<Address> ListOfAddressesByUser(string userName)
        {
            var user = this.pandaDbContext
                .Users
                .Where(u => u.UserName == userName)
                .FirstOrDefault();

            var list = this.pandaDbContext
                .Addresses
                .Where(a => a.UserId == user.Id)
                .OrderBy(a => a.AddressType)
                .Include(a => a.Flat)
                .ToList();
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
    }
}
