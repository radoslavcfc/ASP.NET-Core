using Panda.Data;
using Panda.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panda.Service
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
    }
}
