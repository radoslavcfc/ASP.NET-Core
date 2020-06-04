using Microsoft.EntityFrameworkCore;
using Panda.Data;
using Panda.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Panda.Services
{
    public class AddressesService : IAddressesService
    {
        private readonly PandaDbContext pandaDbContext;

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
    }
}
